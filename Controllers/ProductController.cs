using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Products.Models;
using Products.Repositories;
using Products.ViewModels;

namespace Products.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductRepository _productRepository;
        private readonly CategoryRepository _categoryRepository;

        public ProductController(ProductRepository productRepository, CategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index(string? category, int page = 1)
        {
            IQueryable<Product> products = _productRepository.Products
                .Include(p => p.Category);


            if (!string.IsNullOrEmpty(category))
            {
                products = products
                    .Where(p => p.Category!.Name.ToLower() == category.ToLower());
            }

            int pageSize = 20;
            int total = products.Count();
            int pages = (int)Math.Ceiling((double)total / pageSize);
            page = page < 1 || page > pages ? 1 : page;
            products = products.Skip((page - 1) * pageSize).Take(pageSize);

            var viewModel = new ProductsTableVM
            {
                Products = products,
                Categories = await _categoryRepository.Categories.ToListAsync(),
                Page = page,
                PageCount = pages,
                Category = category
            };

            return View(viewModel);
        }
    }
}
