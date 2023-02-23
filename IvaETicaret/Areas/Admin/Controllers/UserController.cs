using IvaETicaret.Data;
using Microsoft.AspNetCore.Mvc;

namespace IvaETicaret.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
      private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var users= _context.ApplicationUsers.ToList();
            var role=_context.Roles.ToList();
            var userRol=_context.UserRoles.ToList();
            foreach (var item in users)
            {
                var roleId = userRol.FirstOrDefault(i => i.UserId == item.Id).RoleId;
                item.Role = role.FirstOrDefault(u => u.Id == roleId).Name;
            }
            return View(users);
        }
    }
}
