using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IvaETicaret.Data;
using IvaETicaret.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace IvaETicaret.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = Diger.Role_Admin)]
    public class DistrictController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DistrictController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customer/District
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Districts.Include(d => d.County);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Customer/District/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Districts == null)
            {
                return NotFound();
            }

            var district = await _context.Districts
                .Include(d => d.County)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (district == null)
            {
                return NotFound();
            }

            return View(district);
        }

        // GET: Customer/District/Create
        public IActionResult Create()
        {
            ViewData["CountyId"] = new SelectList(_context.Counties, "Id", "Name");
            return View();
        }

        // POST: Customer/District/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CountyId")] District district)
        {
            if (ModelState.IsValid)
            {
                _context.Add(district);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountyId"] = new SelectList(_context.Counties, "Id", "Name", district.CountyId);
            return View(district);
        }

        // GET: Customer/District/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Districts == null)
            {
                return NotFound();
            }

            var district = await _context.Districts.FindAsync(id);
            if (district == null)
            {
                return NotFound();
            }
            ViewData["CountyId"] = new SelectList(_context.Counties, "Id", "Name", district.CountyId);
            return View(district);
        }

        // POST: Customer/District/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CountyId")] District district)
        {
            if (id != district.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(district);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistrictExists(district.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountyId"] = new SelectList(_context.Counties, "Id", "Name", district.CountyId);
            return View(district);
        }

        // GET: Customer/District/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Districts == null)
            {
                return NotFound();
            }

            var district = await _context.Districts
                .Include(d => d.County)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (district == null)
            {
                return NotFound();
            }

            return View(district);
        }

        // POST: Customer/District/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Districts == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Districts'  is null.");
            }
            var district = await _context.Districts.FindAsync(id);
            if (district != null)
            {
                _context.Districts.Remove(district);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DistrictExists(int id)
        {
          return (_context.Districts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
