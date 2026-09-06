using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Products.Models;
using System.Text.Json;

namespace Products.Initializer
{
    public static class Seeder
    {
        public static void Seed(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.Migrate();

            if (!roleManager.Roles.Any())
            {
                var adminRole = new IdentityRole
                {
                    Name = "admin"
                };

                var userRole = new IdentityRole
                {
                    Name = "user"
                };

                roleManager.CreateAsync(adminRole).Wait();
                roleManager.CreateAsync(userRole).Wait();
            }

            if (!userManager.Users.Any())
            {
                var admin = new ApplicationUser
                {
                    Email = "admin@mail.com",
                    UserName = "admin@mail.com",
                    EmailConfirmed = true
                };

                var user = new ApplicationUser
                {
                    Email = "user@mail.com",
                    UserName = "user@mail.com",
                    EmailConfirmed = true
                };

                userManager.CreateAsync(admin, "qwerty").Wait();
                userManager.CreateAsync(user, "qwerty").Wait();

                userManager.AddToRoleAsync(admin, "admin").Wait();
                userManager.AddToRoleAsync(user, "user").Wait();
            }

            if (!context.Categories.Any())
            {
                var filePath = Path.Combine(env.WebRootPath, "jsonData", "CategoriesAndProducts.json");

                if (!File.Exists(filePath))
                {
                    return;
                }

                var json = File.ReadAllText(filePath);

                var categories = JsonSerializer.Deserialize<List<Category>>(json);

                if (categories != null)
                {
                    context.Categories.AddRange(categories);
                    context.SaveChanges();
                }
            }
        }
    }
}
