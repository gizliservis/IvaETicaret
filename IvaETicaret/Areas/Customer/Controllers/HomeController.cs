using IvaETicaret.Data;
using IvaETicaret.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Security.Claims;
using X.PagedList;

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
        public IActionResult Search(string q,int p=1)
        {
            if (!String.IsNullOrEmpty(q))
            {
                var ara=_db.Products.Where(c=>c.Title.Contains(q)|| c.Description.Contains(q));
                if (ara.ToList().Count()>0)
                {
                    var bag = _db.Categories.FirstOrDefault(c => c.Id == ara.FirstOrDefault().CategoryId);
                    var department = _db.Departments.FirstOrDefault(c => c.Id == bag.DepartmentId);
                    ViewBag.Id = department.Id;
                }
                else
                {
                    return RedirectToAction("Index");
                }
                return View(ara.ToPagedList(p,40));

            }
            return View();
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
        public IActionResult Category(int id,int p=1)
        {
            const int pageSize = 1;
            var category = _db.Categories.FirstOrDefault(c => c.DepartmentId == id);
            var cate=_db.Categories.Where(c=>c.DepartmentId== id).ToList();
            PagedList<Product> product=new PagedList<Product>(null,p,40);
            if (cate.Count()>0)
            {
                foreach (var item in cate)
                {
                  product =new PagedList<Product>( _db.Products.Where(c => c.CategoryId == item.Id).ToList(),p,40);
                }
              
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                if (claim != null)
                {
                    var count = _db.ShoppingKarts.Where(c => c.ApplicationUserId == claim.Value).ToList().Count();
                    HttpContext.Session.SetInt32(Diger.ssShopingCart, count);

                }
                ViewBag.id = id;
      
                    return View(product.ToPagedList(p,40));
                
            }
            
         return RedirectToAction("Index");
           
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
        [Authorize]
        public IActionResult Details(ShoppingKart scart)
        {
            scart.Id = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (ModelState.IsValid)
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
        public IActionResult CategoryDetails(int? Id, int departmentId,int p=1)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _db.ShoppingKarts.Where(c => c.ApplicationUserId == claim.Value).ToList().Count();
                HttpContext.Session.SetInt32(Diger.ssShopingCart, count);

            }
            if (Id!=null)
            {
                var product = _db.Products.Where(i => i.CategoryId == Id);

                ViewBag.KategoriId = Id;
                ViewBag.DepartmentId = departmentId;
                return View(product.ToPagedList(p, 40));
            }
            else
            {
                PagedList<Product> product = new PagedList<Product>(null, p, 40);
                var categori = _db.Categories.Where(c => c.DepartmentId == departmentId).ToList();
                foreach (var item in categori)
                {
                    product = new PagedList<Product>(_db.Products.Where(i => i.CategoryId == item.Id).ToList(), p, 40);
                   

                }


                ViewBag.KategoriId = Id;
                ViewBag.DepartmentId = departmentId;
                return View(product.ToPagedList(p, 40));
            }

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