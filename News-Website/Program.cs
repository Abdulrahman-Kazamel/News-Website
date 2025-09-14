using System.Net.Mail;
using NewsWebsite.Core.Models;

namespace NewsWebsite
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ------------------------------------------------------------
            // Database Configuration
            // ------------------------------------------------------------
            var connectionString = builder.Configuration.GetConnectionString("NewsConnection")
                ?? throw new InvalidOperationException("Connection string 'NewsConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            //Register my Repositories
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<INewsPostRepository, NewsPostRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            builder.Services.AddScoped<UploadImageService>();


            // add logging 
            builder.Services.AddLogging(cfg =>
            {
                //cfg.AddDebug();
                cfg.AddConsole();
            });
            // ------------------------------------------------------------
            // Identity Configuration
            // ------------------------------------------------------------
            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = true;
            });

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // ------------------------------------------------------------
            // Database Seed: Roles + Admin User
            // ------------------------------------------------------------
            using(var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                // Ensure roles exist and create them
                string[] roleNames = { "Admin", "default" };
                foreach(var roleName in roleNames)
                {
                    if(!await roleManager.RoleExistsAsync(roleName))
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                }

                // Admin credentials
                var adminEmail = "admin@example.com";
                var adminPassword = "Admin@1234";



                // Create  admin
                var adminInDb = await userManager.FindByEmailAsync(adminEmail);
                if(adminInDb == null)
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    var createResult = await userManager.CreateAsync(adminUser, adminPassword);
                    if(createResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");

                    }
                }

                // ------------------------------------------------------------
                // Middleware Pipeline
                // ------------------------------------------------------------
                if(app.Environment.IsDevelopment())
                {
                    app.UseMigrationsEndPoint();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthentication();
                app.UseAuthorization();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                app.MapRazorPages();

                app.Run();
            }
        }
    }
}