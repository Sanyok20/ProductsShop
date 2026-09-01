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

        public async Task<string?> CreateAsync(ProductCreateVM vm)
        {
            bool res = await IsExistsAsync(vm.Name!);

            if (res)
            {
                return $"Продукт '{vm.Name} вже існує";
            }

            var model = new Product
            {
                Name = vm.Name!,
                Price = vm.Price,
                Description = vm.Description,
                CategoryId = vm.CategoryId
            };

            if (vm.Image != null)
            {
                model.Image = await _imageService.SaveImageAsync(vm.Image, _imagesPath);
            }

            await _context.Products.AddAsync(model);
            await _context.SaveChangesAsync();

            return null;

        }

        public async Task<bool> IsExistsAsync(string name, int id = 0)
        {
            return await _context.Products
                .AnyAsync(c => c.Name.ToLower() == name.ToLower() && c.Id != id);
        }

        public async Task<string?> UpdateAsync(ProductUpdateVM vm)
        {
            bool res = await IsExistsAsync(vm.Name!, vm.Id);

            if (res)
            {
                return $"Продукт '{vm.Name} вже існує";
            }

            var product = await GetByIdAsync(vm.Id);

            if (product == null)
            {
                return $"Продукт з id '{vm.Id}' не існує";
            }

            product.Description = vm.Description;
            product.Price = vm.Price;
            product.Name = vm.Name!;

            if (vm.Image != null)
            {
                if (product.Image != null)
                {
                    string imagePath = Path.Combine(_imagesPath, product.Image);
                    _imageService.DeleteImage(imagePath);
                }

                product.Image = await _imageService.SaveImageAsync(vm.Image, _imagesPath);

            }

            await _context.SaveChangesAsync();

            return null;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);

            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.Image))
                {
                    string imagePath = Path.Combine(_imagesPath, product.Image);
                    _imageService.DeleteImage(imagePath);
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }
    }
}
