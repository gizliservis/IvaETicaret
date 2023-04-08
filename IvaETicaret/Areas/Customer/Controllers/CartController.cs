using IvaETicaret.Data;
using IvaETicaret.Email;
using IvaETicaret.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace IvaETicaret.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(ApplicationDbContext db, IEmailSender emailSender, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _emailSender = emailSender;
            _userManager = userManager;
        }
        public IActionResult Sumary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new OrderHeader(),
                ListCart = _db.ShoppingKarts.Where(i => i.ApplicationUserId == claim.Value).Include(i => i.Product)

            };

            ShoppingCartVM.OrderHeader.OrderTotal = 0;
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == claim.Value);
            ShoppingCartVM.OrderHeader.ApplicationUser = user;

            ShoppingCartVM.OrderHeader.Name = user.Name;
            ShoppingCartVM.OrderHeader.SurName = user.Surname;
            foreach (var item in ShoppingCartVM.ListCart)
            {
                item.Price = item.Product.Price;
                ShoppingCartVM.OrderHeader.OrderTotal += (item.Count * item.Product.Price);

            }
            List<SelectListItem> Adress = _db.Adress.Where(c => c.ApplicationUserId == claim.Value).Select(c => new SelectListItem
            {
                Text = c.AdressTitle,
                Value = c.Id.ToString()
            }).ToList();
            List<SelectListItem> odemeTur = _db.OdemeTurleri.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).ToList();
            ViewBag.addressId = Adress;
            ViewBag.odemeTur = odemeTur;
            return View(ShoppingCartVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Sumary(ShoppingCartVM model)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM.ListCart = _db.ShoppingKarts.Where(c => c.ApplicationUserId == claim.Value).Include(c => c.Product);
            ShoppingCartVM.OrderHeader.OrderStatus = Diger.Durum_Beklemede;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            _db.OrderHeaders.Add(ShoppingCartVM.OrderHeader);
            _db.SaveChanges();
            foreach (var item in ShoppingCartVM.ListCart)
            {
                item.Price = item.Product.Price;
                OrderDetail orderDetail = new OrderDetail
                {
                    ProductId=item.ProductId,
                    OrderId=ShoppingCartVM.OrderHeader.Id,
                    Price=item.Price,
                    Count=item.Count,

                };
                ShoppingCartVM.OrderHeader.OrderTotal += item.Count * item.Product.Price;
                model.OrderHeader.OrderTotal += item.Count * item.Product.Price;
                _db.OrderDetails.Add(orderDetail);

            }
            _db.ShoppingKarts.RemoveRange(ShoppingCartVM.ListCart);
            _db.SaveChanges();
            HttpContext.Session.SetInt32(Diger.ssShopingCart, 0);
            return RedirectToAction("SiparisTamam");

        }
        public IActionResult SiparisTamam()
        {
            return View();
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new OrderHeader(),
                ListCart = _db.ShoppingKarts.Where(i => i.ApplicationUserId == claim.Value).Include(i => i.Product)

            };

            ShoppingCartVM.OrderHeader.OrderTotal = 0;
            ShoppingCartVM.OrderHeader.ApplicationUser = _db.ApplicationUsers.FirstOrDefault(c => c.Id == claim.Value);
            foreach (var item in ShoppingCartVM.ListCart)
            {
                ShoppingCartVM.OrderHeader.OrderTotal += (item.Count * item.Product.Price);
            }
            return View(ShoppingCartVM);
        }
        [HttpPost]
        [ActionName("Index")]
        public async Task<IActionResult> IndexPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == claim.Value);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Doğrulama Emaili Boş");
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code },
                protocol: Request.Scheme);

            SenderEmail.Gonder("E-Postanızı Onaylayın", $"Hesabınızı Onaylamak İçin Tıklayın <a class='btn btn-success' href='{HtmlEncoder.Default.Encode(callbackUrl)}'>Buraya Tıkla</a>.", user.Email
               );
            ModelState.AddModelError(string.Empty, "Email Doğrulama Kodu Gönder.");
            return RedirectToAction("Success");
        }
        public IActionResult Success()
        {
            return View();
        }
        public IActionResult Add(int cardId)
        {
            var cart = _db.ShoppingKarts.FirstOrDefault(i => i.Id == cardId);
            cart.Count += 1;
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Decrease(int cardId)
        {
            var cart = _db.ShoppingKarts.FirstOrDefault(i => i.Id == cardId);
            if (cart.Count == 1)
            {
                var count = _db.ShoppingKarts.Where(c => c.ApplicationUserId == cart.ApplicationUserId).ToList().Count();
                _db.ShoppingKarts.Remove(cart);
                _db.SaveChanges();
                HttpContext.Session.SetInt32(Diger.ssShopingCart, count - 1);
            }
            else
            {
                cart.Count -= 1;
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Remove(int cardId)
        {
            var cart = _db.ShoppingKarts.FirstOrDefault(i => i.Id == cardId);

            var count = _db.ShoppingKarts.Where(c => c.ApplicationUserId == cart.ApplicationUserId).ToList().Count();
            _db.ShoppingKarts.Remove(cart);
            _db.SaveChanges();
            HttpContext.Session.SetInt32(Diger.ssShopingCart, count - 1);


            return RedirectToAction(nameof(Index));
        }

    }
}
