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
            //Register my Repositories
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<INewsPostRepository, NewsPostRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

            builder.Services.AddScoped<UploadImageService>();

            // ------------------------------------------------------------
            // Identity Configuration
            // ------------------------------------------------------------
            builder.Services
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

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
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

                // Ensure roles exist
                string[] roleNames = { "Admin", "default" };
                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                }

                // Admin credentials
                var adminEmail = "admin@example.com";
                var adminPassword = "Admin@1234";

                // Delete old admin if exists
                var oldAdmin = await userManager.FindByEmailAsync(adminEmail);
                if (oldAdmin != null)
                {
                    await userManager.DeleteAsync(oldAdmin);
                    Console.WriteLine("🗑️ Old admin user deleted.");
                }

                // Create fresh admin
                var adminUser = new IdentityUser
                {
                    UserName = adminEmail, // ✅ match email to username
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(adminUser, adminPassword);
                if (!createResult.Succeeded)
                {
                    throw new Exception("❌ Failed to create admin user: " +
                        string.Join(", ", createResult.Errors.Select(e => e.Description)));
                }
                else
                {
                    Console.WriteLine($"✅ Admin created: {adminEmail} / {adminPassword}");
                }

                // Assign Admin role
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    Console.WriteLine("✅ Admin role assigned.");
                }
            }

            // ------------------------------------------------------------
            // Middleware Pipeline
            // ------------------------------------------------------------
            if (app.Environment.IsDevelopment())
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
