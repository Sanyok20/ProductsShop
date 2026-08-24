using Microsoft.AspNetCore.Mvc;
using Products.Models;

namespace Products.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var ProdcutsFromFirstCategory = _context.Products
                .Where(p => p.CategoryId < 3)
                .ToList();

            return View(ProdcutsFromFirstCategory);
        }
    }
}
