using IvaETicaret.Data;
using IvaETicaret.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace IvaETicaret.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        public int idd;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var department = _db.Departments.ToList();
            return View(department);
        }
        public IActionResult Category(int id)
        {
            var category = _db.Categories.FirstOrDefault(c => c.DepartmentId == id);
            var product = _db.Products.Where(c => c.CategoryId == category.Id);
            ViewBag.id = id;
            return View(product);
        }
        public IActionResult Details(int id)
        {
            var product = _db.Products.FirstOrDefault(c => c.Id == id);
            ShoppingKart cart = new ShoppingKart()
            {
                Product=product,
                ProductId=product.Id,
            };
            return View(cart);
        }
        public IActionResult CategoryDetails(int? Id, int departmentId)
        {
            var product = _db.Products.Where(i => i.CategoryId == Id).ToList();
            ViewBag.KategoriId = Id;
            ViewBag.DepartmentId = departmentId;
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}