using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CFE.Models;

namespace CFE.Controllers
{
    public class AreasController : Controller
    {
        private readonly empresaContext _context;

        public AreasController(empresaContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,User,Moder")]
        public async Task<IActionResult> Index()
        {
            return _context.Areas != null ?
                View(await _context.Areas.ToListAsync()) :
                Problem("Entity set 'empresaContext.Areas' is null.");
        }

        [Authorize(Roles = "Admin,User,Moder")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Areas == null) return NotFound();

            var area = await _context.Areas.FirstOrDefaultAsync(m => m.IdAreas == id);
            if (area == null) return NotFound();

            return View(area);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("IdAreas,DescripcionArea")] Area area)
        {
            if (ModelState.IsValid)
            {
                _context.Add(area);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(area);
        }

        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Areas == null) return NotFound();

            var area = await _context.Areas.FindAsync(id);
            if (area == null) return NotFound();

            ViewData["AreasList"] = await _context.Areas.ToListAsync();
            return View(area);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Edit(int id, [Bind("IdAreas,DescripcionArea")] Area area)
        {
            if (id != area.IdAreas) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(area);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AreaExists(area.IdAreas)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(area);
        }

        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Areas == null) return NotFound();

            var area = await _context.Areas.FirstOrDefaultAsync(m => m.IdAreas == id);
            if (area == null) return NotFound();

            return View(area);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Areas == null)
                return Problem("Entity set 'empresaContext.Areas' is null.");

            var area = await _context.Areas.FindAsync(id);
            if (area != null)
            {
                _context.Areas.Remove(area);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AreaExists(int id)
        {
            return (_context.Areas?.Any(e => e.IdAreas == id)).GetValueOrDefault();
        }
    }
}