using Microsoft.AspNetCore.Mvc;

namespace Products.Controllers
{
    public class FirstProductController : Controller
    {
        private readonly AppDbContext _context;

        public FirstProductController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var FirstProductInDb = _context.Products
                .FirstOrDefault(p => p.CategoryId == 1);

            return View(FirstProductInDb);
        }
    }
}

