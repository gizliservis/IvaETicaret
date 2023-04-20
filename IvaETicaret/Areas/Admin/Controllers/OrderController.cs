using IvaETicaret.Data;
using IvaETicaret.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IvaETicaret.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public OrderDetailsVM OrderVM { get; set; }
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Details(int id)
        {
            OrderVM = new OrderDetailsVM
            {
                OrderHeader = _db.OrderHeaders.Include(c=>c.Adress).Include(c=>c.OdemeTur).Include(c=>c.ApplicationUser).FirstOrDefault(c => c.Id == id),
                OrderDetails = _db.OrderDetails.Where(c => c.OrderId == id).Include(c => c.Product),
            };
            return View(OrderVM);
        }
        public IActionResult Hazırlaniyor()
        {
            OrderHeader orderHeader = _db.OrderHeaders.FirstOrDefault(i => i.Id == OrderVM.OrderHeader.Id);
            orderHeader.OrderStatus = Diger.Durum_Hazirlaniyor;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult YolaCikart()
        {
            OrderHeader orderHeader = _db.OrderHeaders.FirstOrDefault(i => i.Id == OrderVM.OrderHeader.Id);
            orderHeader.OrderStatus = Diger.Durum_YolaCikti;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult TeslimEt()
        {
            OrderHeader orderHeader = _db.OrderHeaders.FirstOrDefault(i => i.Id == OrderVM.OrderHeader.Id);
            orderHeader.OrderStatus = Diger.Durum_TeslimEdildi;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            IEnumerable<OrderHeader> orderHeaderList;
            if (User.IsInRole(Diger.Role_Admin))
            {
                orderHeaderList = _db.OrderHeaders.Include(c=>c.OdemeTur).Include(c=>c.ApplicationUser).Include(c=>c.Adress).ToList();
            }
            else if (User.IsInRole(Diger.Role_User)||User.IsInRole(Diger.Role_Birey))
            {
                orderHeaderList = _db.OrderHeaders.Where(i => i.ApplicationUserId == claim.Value).Include(i => i.ApplicationUser).Include(c=>c.OdemeTur);

            }
            else
            {
                var bayi = _db.ApplicationUsers.Where(i => i.Id == claim.Value).FirstOrDefault().BranchId;
                orderHeaderList = _db.OrderHeaders.Include(i => i.Adress).Where(i => i.Adress.BranchId == bayi).Include(c=>c.OdemeTur).Include(c=>c.ApplicationUser);
            }
            return View(orderHeaderList);
        }
        public IActionResult Beklenen()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            IEnumerable<OrderHeader> orderHeaderList;
            if (User.IsInRole(Diger.Role_Admin))
            {
                orderHeaderList = _db.OrderHeaders.Include(c => c.OdemeTur).Include(c => c.ApplicationUser).Include(c => c.Adress).Where(c=>c.OrderStatus==Diger.Durum_Beklemede).ToList();
            }
            else if (User.IsInRole(Diger.Role_User) || User.IsInRole(Diger.Role_Birey))
            {
                orderHeaderList = _db.OrderHeaders.Where(i => i.ApplicationUserId == claim.Value && i.OrderStatus==Diger.Durum_Beklemede).Include(i => i.ApplicationUser).Include(c => c.OdemeTur);

            }
            else
            {
                var bayi = _db.ApplicationUsers.Where(i => i.Id == claim.Value).FirstOrDefault().BranchId;
                orderHeaderList = _db.OrderHeaders.Include(i => i.Adress).Include(c => c.OdemeTur).Include(c => c.ApplicationUser).Where(i => i.Adress.BranchId == bayi&& i.OrderStatus==Diger.Durum_Beklemede);
            }
            return View(orderHeaderList);
        }
        public IActionResult Hazirlanan()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            IEnumerable<OrderHeader> orderHeaderList;
            if (User.IsInRole(Diger.Role_Admin))
            {
                orderHeaderList = _db.OrderHeaders.Include(c => c.OdemeTur).Include(c => c.ApplicationUser).Include(c => c.Adress).Where(c => c.OrderStatus == Diger.Durum_Hazirlaniyor).ToList();
            }
            else if (User.IsInRole(Diger.Role_User) || User.IsInRole(Diger.Role_Birey))
            {
                orderHeaderList = _db.OrderHeaders.Where(i => i.ApplicationUserId == claim.Value && i.OrderStatus == Diger.Durum_Hazirlaniyor).Include(i => i.ApplicationUser).Include(c => c.OdemeTur);

            }
            else
            {
                var bayi = _db.ApplicationUsers.Where(i => i.Id == claim.Value).FirstOrDefault().BranchId;
                orderHeaderList = _db.OrderHeaders.Include(i => i.Adress).Include(c => c.OdemeTur).Include(c => c.ApplicationUser).Where(i => i.Adress.BranchId == bayi && i.OrderStatus == Diger.Durum_Hazirlaniyor);
            }
            return View(orderHeaderList);
        }
        public IActionResult YolaCikan()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            IEnumerable<OrderHeader> orderHeaderList;
            if (User.IsInRole(Diger.Role_Admin))
            {
                orderHeaderList = _db.OrderHeaders.Include(c => c.OdemeTur).Include(c => c.ApplicationUser).Include(c => c.Adress).Where(c => c.OrderStatus == Diger.Durum_YolaCikti).ToList();
            }
            else if (User.IsInRole(Diger.Role_User) || User.IsInRole(Diger.Role_Birey))
            {
                orderHeaderList = _db.OrderHeaders.Where(i => i.ApplicationUserId == claim.Value && i.OrderStatus == Diger.Durum_YolaCikti).Include(i => i.ApplicationUser).Include(c => c.OdemeTur);

            }
            else
            {
                var bayi = _db.ApplicationUsers.Where(i => i.Id == claim.Value).FirstOrDefault().BranchId;
                orderHeaderList = _db.OrderHeaders.Include(i => i.Adress).Include(c => c.OdemeTur).Include(c => c.ApplicationUser).Where(i => i.Adress.BranchId == bayi && i.OrderStatus == Diger.Durum_YolaCikti);
            }
            return View(orderHeaderList);
        }
        public IActionResult TeslimEdildi()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            IEnumerable<OrderHeader> orderHeaderList;
            if (User.IsInRole(Diger.Role_Admin))
            {
                orderHeaderList = _db.OrderHeaders.Include(c => c.OdemeTur).Include(c => c.ApplicationUser).Include(c => c.Adress).Where(c => c.OrderStatus == Diger.Durum_TeslimEdildi).ToList();
            }
            else if (User.IsInRole(Diger.Role_User) || User.IsInRole(Diger.Role_Birey))
            {
                orderHeaderList = _db.OrderHeaders.Where(i => i.ApplicationUserId == claim.Value && i.OrderStatus == Diger.Durum_TeslimEdildi).Include(i => i.ApplicationUser).Include(c => c.OdemeTur);

            }
            else
            {
                var bayi = _db.ApplicationUsers.Where(i => i.Id == claim.Value).FirstOrDefault().BranchId;
                orderHeaderList = _db.OrderHeaders.Include(i => i.Adress).Include(c => c.OdemeTur).Include(c => c.ApplicationUser).Where(i => i.Adress.BranchId == bayi && i.OrderStatus == Diger.Durum_TeslimEdildi);
            }
            return View(orderHeaderList);
        }
    }
}
