using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NewsWebsite.Core.Context;

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

            // ------------------------------------------------------------
            // Identity Configuration
            // ------------------------------------------------------------
            builder.Services
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });
 


            // ------------------------------------------------------------
            // MVC / Razor / Developer Tools
            // ------------------------------------------------------------
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // ------------------------------------------------------------
            // Database Seed: Roles + Admin User
            // ------------------------------------------------------------
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

                // Ensure admin user exists
                var adminEmail = "admin@admin.com";
                var adminPassword = "Admin@123";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    adminUser = new IdentityUser
                    {
                        UserName = "admin",
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    var createResult = await userManager.CreateAsync(adminUser, adminPassword);
                    if (!createResult.Succeeded)
                    {
                        throw new Exception("Failed to create admin user: " +
                            string.Join(", ", createResult.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    // Reset password if user already exists
                    var resetToken = await userManager.GeneratePasswordResetTokenAsync(adminUser);
                    var resetResult = await userManager.ResetPasswordAsync(adminUser, resetToken, adminPassword);

                    if (!resetResult.Succeeded)
                    {
                        // If password reset fails, log details (to DB/console)
                        Console.WriteLine("Failed to reset password for admin: " +
                            string.Join(", ", resetResult.Errors.Select(e => e.Description)));
                    }
                }

                // Ensure admin user has Admin role
                if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
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

            // ------------------------------------------------------------
            // Routing
            // ------------------------------------------------------------
            // Optional Admin Area Route
            //app.MapControllerRoute(
            //    name: "Admin",
            //    pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}
