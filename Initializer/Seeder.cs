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

            context.Database.Migrate();

            string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "jsonData");

            if (!context.Categories.Any())
            {
                var jsonPath = Path.Combine(dirPath, "categories.json");

                var json = File.ReadAllText(jsonPath);

                var categories = JsonSerializer.Deserialize<List<Category>>(json);
                if (categories != null)
                {
                    context.Categories.AddRange(categories);
                    context.SaveChanges();
                }
            }

            if(!context.Products.Any())
            {
                var jsonPath = Path.Combine(dirPath, "products.json");

                var json = File.ReadAllText(jsonPath);

                var products = JsonSerializer.Deserialize<List<Product>>(json);
                if(products != null)
                {
                    context.Products.AddRange(products);
                    context.SaveChanges();
                }
            }
        }
    }
}
