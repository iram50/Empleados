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
    public class InstructorsController : Controller
    {
        private readonly empresaContext _context;

        public InstructorsController(empresaContext context)
        {
            _context = context;
        }

        // GET: Instructors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Instructors.ToListAsync());
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Instructors == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.Id_Instructor == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructors/Create
        public IActionResult Create()
        {
            // Cargar todos los instructores para la lista lateral
            var instructoresList = _context.Instructors.ToList();
            ViewData["InstructoresList"] = instructoresList;

            // Crear un nuevo modelo con Id_Instructor = 0
            return View(new Instructor { Id_Instructor = 0 });
        }

        // POST: Instructors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NombreInstructor,Descripcion")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Si hay errores, recargar los datos necesarios
            var instructoresList = _context.Instructors.ToList();
            ViewData["InstructoresList"] = instructoresList;

            return View(instructor);
        }

        // GET: Instructors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Instructors == null)
            {
                return NotFound();
            }

            // Cargar todos los instructores para la lista lateral
            var instructoresList = await _context.Instructors.ToListAsync();
            ViewData["InstructoresList"] = instructoresList;

            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id_Instructor,NombreInstructor,Descripcion")] Instructor instructor)
        {
            if (id != instructor.Id_Instructor)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instructor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.Id_Instructor))
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
            var instructoresList = await _context.Instructors.ToListAsync();
            ViewData["InstructoresList"] = instructoresList;

            return View(instructor);
        }

        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Instructors == null)
            {
                return NotFound();
            }

            var instructor = await _context.Instructors
                .FirstOrDefaultAsync(m => m.Id_Instructor == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Instructors == null)
            {
                return Problem("Entity set 'empresaContext.Instructors' is null.");
            }

            var instructor = await _context.Instructors.FindAsync(id);

            if (instructor != null)
            {
                // Primero, obtener los cursos que están ligados a este instructor
                var cursosRelacionados = _context.Cursos.Where(c => c.Id_Instructor == id);

                // Actualizar cada curso para que su Id_Instructor sea NULL
                foreach (var curso in cursosRelacionados)
                {
                    curso.Id_Instructor = null;
                }

                // Ahora sí, eliminar el instructor
                _context.Instructors.Remove(instructor);

                // Guardar todos los cambios juntos
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool InstructorExists(int id)
        {
            return (_context.Instructors?.Any(e => e.Id_Instructor == id)).GetValueOrDefault();
        }
    }
}