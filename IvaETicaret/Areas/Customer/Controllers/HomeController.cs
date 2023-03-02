using IvaETicaret.Data;
using IvaETicaret.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

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
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _db.ShoppingKarts.Where(c => c.ApplicationUserId == claim.Value).ToList().Count();
                HttpContext.Session.SetInt32(Diger.ssShopingCart, count);

            }
            return View(department);
        }
        public IActionResult Category(int id)
        {
            var category = _db.Categories.FirstOrDefault(c => c.DepartmentId == id);
            var product = _db.Products.Where(c => c.CategoryId == category.Id);
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim!=null)
            {
                var count = _db.ShoppingKarts.Where(c => c.ApplicationUserId == claim.Value).ToList().Count();
                HttpContext.Session.SetInt32(Diger.ssShopingCart, count);

            }
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
                Price=product.Price,
             
            };
            return View(cart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize]
        public IActionResult Details(ShoppingKart scart)
        {
            scart.Id = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
            {
                if (claim.Value==null)
                {
                    scart.ApplicationUserId = claim.Value;
                    ShoppingKart cart = _db.ShoppingKarts.FirstOrDefault(c => c.ApplicationUserId == scart.ApplicationUserId && c.ProductId == scart.ProductId);
                    if (cart == null)
                    {
                        _db.ShoppingKarts.Add(scart);
                    }
                    else
                    {
                        cart.Count += scart.Count;

                    }
                    _db.SaveChanges();
                    var count = _db.ShoppingKarts.Where(i => i.ApplicationUserId == scart.ApplicationUserId).ToList().Count();
                    HttpContext.Session.SetInt32(Diger.ssShopingCart, count);
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("/Identity/Account/Login");
                }
            
             
            }
            else
            {
                var product = _db.Products.FirstOrDefault(c => c.Id == scart.Id);
                ShoppingKart cart = new ShoppingKart()
                {
                    Product = product,
                    ProductId = product.Id,
                    Price = product.Price,
                };
            }

            return View(scart);
        }
        public IActionResult CategoryDetails(int? Id, int departmentId)
        {
            var product = _db.Products.Where(i => i.CategoryId == Id).ToList();
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _db.ShoppingKarts.Where(c => c.ApplicationUserId == claim.Value).ToList().Count();
                HttpContext.Session.SetInt32(Diger.ssShopingCart, count);

            }
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