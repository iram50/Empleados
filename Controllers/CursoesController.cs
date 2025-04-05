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
    public class CursoesController : Controller
    {
        private readonly empresaContext _context;

        public CursoesController(empresaContext context)
        {
            _context = context;
        }

        // GET: Cursoes
        public async Task<IActionResult> Index()
        {
            var cursos = await _context.Cursos.Include(c => c.Instructor).ToListAsync();
            return View(cursos);
        }

        // GET: Cursoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Cursos == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                .FirstOrDefaultAsync(m => m.IdCurso == id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        public IActionResult Create()
        {
            // Verifica si la base de datos tiene instructores
            var instructores = _context.Instructors.ToList();

            if (instructores == null || instructores.Count == 0)
            {
                Console.WriteLine("No hay instructores en la base de datos.");
                ViewBag.Instructores = new SelectList(new List<Instructor>());
            }
            else
            {
                Console.WriteLine($"Se encontraron {instructores.Count} instructores.");
                ViewBag.Instructores = new SelectList(instructores, "Id_Instructor", "NombreInstructor");
            }

            return View();
        }


        // POST: Cursoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreCurso,Id_Instructor")] Curso curso)
        {

            _context.Add(curso);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }



        // GET: Cursoes/Edit/5 
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Cursos == null)
            {
                return NotFound();
            }

            // Cargar todos los cursos para la lista lateral
            var cursosList = await _context.Cursos
                .Include(c => c.Instructor)
                .ToListAsync();

            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }

            // Lista de instructores desde la base de datos
            var instructores = await _context.Instructors.ToListAsync();

            if (instructores == null || instructores.Count == 0)
            {
                ViewBag.Instructores = new SelectList(new List<Instructor>());
            }
            else
            {
                ViewBag.Instructores = new SelectList(instructores, "Id_Instructor", "NombreInstructor", curso.Id_Instructor);
            }

            // Pasar la lista de cursos a la vista
            ViewData["CursosList"] = cursosList;

            return View(curso);
        }


        // POST: Cursoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCurso,NombreCurso,Id_Instructor")] Curso curso)
        {
            if (id != curso.IdCurso)
            {
                return NotFound();
            }
            try
            {
                _context.Update(curso);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursoExists(curso.IdCurso))
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

        // GET: Cursoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Cursos == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                .FirstOrDefaultAsync(m => m.IdCurso == id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // POST: Cursoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Cursos == null)
            {
                return Problem("Entity set 'empresaContext.Cursos'  is null.");
            }
            var curso = await _context.Cursos.FindAsync(id);
            if (curso != null)
            {
                _context.Cursos.Remove(curso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CursoExists(int id)
        {
            return (_context.Cursos?.Any(e => e.IdCurso == id)).GetValueOrDefault();
        }
    }
}