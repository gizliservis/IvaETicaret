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
    public class CountyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CountyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customer/County
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Counties.Include(c => c.City);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Customer/County/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Counties == null)
            {
                return NotFound();
            }

            var county = await _context.Counties
                .Include(c => c.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (county == null)
            {
                return NotFound();
            }

            return View(county);
        }

        // GET: Customer/County/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name");
            return View();
        }

        // POST: Customer/County/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CityId")] County county)
        {
            if (ModelState.IsValid)
            {
                _context.Add(county);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Name", county.CityId);
            return View(county);
        }

        // GET: Customer/County/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Counties == null)
            {
                return NotFound();
            }

            var county = await _context.Counties.FindAsync(id);
            if (county == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id", county.CityId);
            return View(county);
        }

        // POST: Customer/County/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CityId")] County county)
        {
            if (id != county.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(county);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountyExists(county.Id))
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
            ViewData["CityId"] = new SelectList(_context.Cities, "Id", "Id", county.CityId);
            return View(county);
        }

        // GET: Customer/County/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Counties == null)
            {
                return NotFound();
            }

            var county = await _context.Counties
                .Include(c => c.City)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (county == null)
            {
                return NotFound();
            }

            return View(county);
        }

        // POST: Customer/County/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Counties == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Counties'  is null.");
            }
            var county = await _context.Counties.FindAsync(id);
            if (county != null)
            {
                _context.Counties.Remove(county);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountyExists(int id)
        {
          return (_context.Counties?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
