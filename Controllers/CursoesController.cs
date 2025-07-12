using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CFE.Models;

namespace CFE.Controllers
{
    public class CursoesController : Controller
    {
        private readonly empresaContext _context;

        public CursoesController(empresaContext context)
        {
            _context = context;
        }

        // GET: Cursoes (todos los roles pueden ver)
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var cursos = await _context.Cursos.Include(c => c.Instructor).ToListAsync();
            return View(cursos);
        }

        // GET: Cursoes/Details/
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cursos == null)
                return NotFound();

            var curso = await _context.Cursos.Include(c => c.Instructor).FirstOrDefaultAsync(m => m.IdCurso == id);
            if (curso == null)
                return NotFound();

            return View(curso);
        }

        // GET: Cursoes/Create (solo Admin)
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewBag.Instructores = new SelectList(_context.Instructors, "Id_Instructor", "NombreInstructor");
            return View();
        }

        // POST: Cursoes/Create (solo Admin)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("NombreCurso,Id_Instructor")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(curso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(curso);
        }

        // GET: Cursoes/Edit/5 (Admin y Moder)
        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null) return NotFound();

            ViewBag.Instructores = new SelectList(_context.Instructors, "Id_Instructor", "NombreInstructor", curso.Id_Instructor);
            ViewData["CursosList"] = await _context.Cursos.Include(c => c.Instructor).ToListAsync();

            return View(curso);
        }

        // POST: Cursoes/Edit/5 (Admin y Moder)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Edit(int id, [Bind("IdCurso,NombreCurso,Id_Instructor")] Curso curso)
        {
            if (id != curso.IdCurso) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(curso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CursoExists(curso.IdCurso))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(curso);
        }

        // GET: Cursoes/Delete/5 (Admin y Moder)
        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var curso = await _context.Cursos.Include(c => c.Instructor).FirstOrDefaultAsync(m => m.IdCurso == id);
            if (curso == null) return NotFound();

            return View(curso);
        }

        // POST: Cursoes/Delete/5 (Admin y Moder)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso != null)
                _context.Cursos.Remove(curso);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.IdCurso == id);
        }
    }
}
