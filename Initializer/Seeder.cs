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

            if(!roleManager.RoleExistsAsync("admin").GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole("admin")).GetAwaiter().GetResult();
            }

            if (!roleManager.RoleExistsAsync("user").GetAwaiter().GetResult())
            {
                roleManager.CreateAsync(new IdentityRole("user")).GetAwaiter().GetResult();
            }

            var adminUser = userManager.FindByEmailAsync("admin@mail.com").GetAwaiter().GetResult();
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    Email = "admin@mail.com",
                    UserName = "admin@mail.com",
                    EmailConfirmed = true
                };
                userManager.CreateAsync(adminUser, "qwerty").GetAwaiter().GetResult();
            }

            if (!userManager.IsInRoleAsync(adminUser, "admin").GetAwaiter().GetResult())
            {
                userManager.AddToRoleAsync(adminUser, "admin").GetAwaiter().GetResult();
            }

            var normalUser = userManager.FindByEmailAsync("user@mail.com").GetAwaiter().GetResult();
            if (normalUser == null)
            {
                normalUser = new ApplicationUser
                {
                    Email = "user@mail.com",
                    UserName = "user@mail.com",
                    EmailConfirmed = true
                };
                userManager.CreateAsync(normalUser, "qwerty").GetAwaiter().GetResult();
            }

            if (!userManager.IsInRoleAsync(normalUser, "user").GetAwaiter().GetResult())
            {
                userManager.AddToRoleAsync(normalUser, "user").GetAwaiter().GetResult();
            }

            if (!context.Categories.Any())
            {
                var filePath = Path.Combine(env.WebRootPath, "jsonData", "CategoriesAndProducts.json");

                if (File.Exists(filePath))
                {
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
}
