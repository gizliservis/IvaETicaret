using IvaETicaret.Data;
using IvaETicaret.Email;
using IvaETicaret.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new OrderHeader(),
                ListCart=_db.ShoppingKarts.Where(i=>i.ApplicationUserId==claim.Value).Include(i=>i.Product)                

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
            var user=_db.ApplicationUsers.FirstOrDefault(c=>c.Id==claim.Value);
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
    }
}
