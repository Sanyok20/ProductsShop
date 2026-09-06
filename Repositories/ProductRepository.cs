using Microsoft.EntityFrameworkCore;
using Products.Models;
using Products.Services;
using Products.ViewModels;

namespace Products.Repositories
{
    public class ProductRepository
    {
        private readonly AppDbContext _context;
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _environment;

        private readonly string _imagesPath;

        public ProductRepository(AppDbContext context, IWebHostEnvironment environment, ImageService imageService)
        {
            _context = context;
            _environment = environment;
            _imageService = imageService;

            string root = _environment.WebRootPath;
            _imagesPath = Path.Combine(root, "images", "categories", "products");
        }

        public IQueryable<Product> Products => _context.Products.AsNoTracking();

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> IsExistsAsync(string name, int id = 0)
        {
            return await _context.Products
                .AnyAsync(c => c.Name.ToLower() == name.ToLower() && c.Id != id);
        }
    }
}