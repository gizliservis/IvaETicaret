using IvaETicaret.Data;
using IvaETicaret.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
        [Authorize(Roles = Diger.Role_User)]
        //[Authorize(Roles = Diger.Role_Admin)]
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
        // GET: Admin/Category/Delete/5
        [Authorize(Roles = Diger.Role_Admin)]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers
                .Include(c => c.Branch)
                .FirstOrDefaultAsync(m => m.Id == id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [Authorize(Roles = Diger.Role_Admin)]
        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.ApplicationUsers == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ApplicationUser'  is null.");
            }
            var user = await _context.ApplicationUsers.FindAsync(id);
            if (user != null)
            {
                _context.ApplicationUsers.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
