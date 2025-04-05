using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CFE.Models;

namespace CFE.Controllers
{
    public class PuestoesController : Controller
    {
        private readonly empresaContext _context;

        public PuestoesController(empresaContext context)
        {
            _context = context;
        }

        // GET: Puestoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Puestos.ToListAsync());
        }

        // GET: Puestoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Puestos == null)
            {
                return NotFound();
            }

            var puesto = await _context.Puestos
                .FirstOrDefaultAsync(m => m.IdPuesto == id);
            if (puesto == null)
            {
                return NotFound();
            }

            return View(puesto);
        }

        // GET: Puestoes/Create
        public IActionResult Create()
        {
            // Cargar todos los puestos para la lista lateral
            var puestosList = _context.Puestos.ToList();
            ViewData["PuestosList"] = puestosList;

            // Crear un nuevo modelo con IdPuesto = 0
            return View(new Puesto { IdPuesto = 0 });
        }

        // POST: Puestoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DescripcionPuesto")] Puesto puesto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(puesto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Si hay errores, recargar los datos necesarios
            var puestosList = _context.Puestos.ToList();
            ViewData["PuestosList"] = puestosList;

            return View(puesto);
        }

        // GET: Puestoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Puestos == null)
            {
                return NotFound();
            }

            // Cargar todos los puestos para la lista lateral
            var puestosList = await _context.Puestos.ToListAsync();
            ViewData["PuestosList"] = puestosList;

            var puesto = await _context.Puestos.FindAsync(id);
            if (puesto == null)
            {
                return NotFound();
            }

            return View(puesto);
        }

        // POST: Puestoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPuesto,DescripcionPuesto")] Puesto puesto)
        {
            if (id != puesto.IdPuesto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(puesto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PuestoExists(puesto.IdPuesto))
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

            // Si hay errores, recargar los datos necesarios
            var puestosList = await _context.Puestos.ToListAsync();
            ViewData["PuestosList"] = puestosList;

            return View(puesto);
        }

        // GET: Puestoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Puestos == null)
            {
                return NotFound();
            }

            var puesto = await _context.Puestos
                .FirstOrDefaultAsync(m => m.IdPuesto == id);
            if (puesto == null)
            {
                return NotFound();
            }

            return View(puesto);
        }

        // POST: Puestoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Puestos == null)
            {
                return Problem("Entity set 'empresaContext.Puestos' is null.");
            }
            var puesto = await _context.Puestos.FindAsync(id);
            if (puesto != null)
            {
                _context.Puestos.Remove(puesto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PuestoExists(int id)
        {
            return (_context.Puestos?.Any(e => e.IdPuesto == id)).GetValueOrDefault();
        }
    }
}