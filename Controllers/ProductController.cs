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

        public ProductController(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public IActionResult Index()
        {
            var ProdcutsFromFirstCategory = _productRepository.Products
                .Where(p => p.CategoryId < 3)
                .ToList();

            return View(ProdcutsFromFirstCategory);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _productRepository.CreateAsync(vm);

            if (result != null)
            {
                ModelState.AddModelError("Name", result);
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return RedirectToAction("Index");
            }

            var vm = new ProductUpdateVM
            {
                Id = id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _productRepository.UpdateAsync(vm);

            if (result != null)
            {
                ModelState.AddModelError("Name", result);
                return View(vm);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _productRepository.DeleteAsync(id);

            return RedirectToAction("Index");
        }
    }
}
