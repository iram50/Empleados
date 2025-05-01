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
    public class GrupoesController : Controller
    {
        private readonly empresaContext _context;

        public GrupoesController(empresaContext context)
        {
            _context = context;
        }

        // GET: Grupoes
        public async Task<IActionResult> Index()
        {
            var empresaContext = _context.Grupos.Include(g => g.IdCursoNavigation);
            return View(await empresaContext.ToListAsync());
        }

        // GET: Grupoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Grupos == null)
            {
                return NotFound();
            }

            var grupo = await _context.Grupos
                .Include(g => g.IdCursoNavigation)
                .FirstOrDefaultAsync(m => m.ClaveGrupo == id);
            if (grupo == null)
            {
                return NotFound();
            }

            return View(grupo);
        }

        // GET: Grupoes/Create
        public IActionResult Create()
        {
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "IdCurso", "NombreCurso");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClaveGrupo,IdCurso,Fechainicial,FechaFinal,Horario,Lugar,Comentarios,Instructor,AreaOfrece,Status")] Grupo grupo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(grupo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "IdCurso", "NombreCurso", grupo.IdCurso);
            return View(grupo);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Grupos == null)
            {
                return NotFound();
            }

            var grupo = await _context.Grupos.FindAsync(id);
            if (grupo == null)
            {
                return NotFound();
            }
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "IdCurso", "NombreCurso", grupo.IdCurso);
            return View(grupo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClaveGrupo,IdCurso,Fechainicial,FechaFinal,Horario,Lugar,Comentarios,Instructor,AreaOfrece,Status")] Grupo grupo)
        {
            if (id != grupo.ClaveGrupo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grupo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GrupoExists(grupo.ClaveGrupo))
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
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "IdCurso", "NombreCurso", grupo.IdCurso);
            return View(grupo);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Grupos == null)
            {
                return NotFound();
            }

            var grupo = await _context.Grupos
                .Include(g => g.IdCursoNavigation)
                .FirstOrDefaultAsync(m => m.ClaveGrupo == id);
            if (grupo == null)
            {
                return NotFound();
            }

            return View(grupo);
        }

        // POST: Grupoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Grupos == null)
            {
                return Problem("Entity set 'empresaContext.Grupos'  is null.");
            }
            var grupo = await _context.Grupos.FindAsync(id);
            if (grupo != null)
            {
                _context.Grupos.Remove(grupo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GrupoExists(int id)
        {
          return (_context.Grupos?.Any(e => e.ClaveGrupo == id)).GetValueOrDefault();
        }
    }
}
