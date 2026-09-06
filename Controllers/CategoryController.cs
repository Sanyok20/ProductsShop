using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Products.Models;
using Products.Repositories;
using Products.ViewModels;

namespace Products.Controllers
{
    [Authorize(Roles = "admin")]
    public class CategoryController : Controller
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryController(CategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categories = _categoryRepository.Categories;

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _categoryRepository.CreateAsync(vm);

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
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                return RedirectToAction("Index");
            }

            var vm = new CategoryUpdateVM
            {
                Id = id,
                Name = category.Name,
                Description = category.Description
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CategoryUpdateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var result = await _categoryRepository.UpdateAsync(vm);

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
            await _categoryRepository.DeleteAsync(id);

            return RedirectToAction("Index");
        }
    }
}