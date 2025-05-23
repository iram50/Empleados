using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CFE.Models;
using CFE.ViewModels;

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
        public IActionResult Index(int? empleadoId)
        {
            var empleados = _context.Empleados.ToList();

            var cursosInscritos = new List<Grupo>();
            if (empleadoId.HasValue)
            {
                cursosInscritos = _context.Grupos
                    .Include(g => g.IdCursoNavigation)
                    .Where(g => g.IdEmpleado == empleadoId)
                    .ToList();

            }

            var cursosDisponibles = _context.Cursos
            .Include(c => c.Instructor)
            .ToList();


            var viewModel = new GruposIndexViewModel
            {
                Empleados = empleados,
                EmpleadoSeleccionadoId = empleadoId,
                CursosInscritos = cursosInscritos,
                CursosDisponibles = cursosDisponibles,
                NuevoGrupo = new Grupo { IdEmpleado = empleadoId ?? 0 }
            };

            return View(viewModel);
        }
        //METODO INSCRIBIR
        [HttpPost]
        public async Task<IActionResult> Inscribir(Grupo nuevoGrupo)
        {
            // 1. Validar el IdInstructor: si no existe en la base de datos o es cero, asignar null
            if (nuevoGrupo.IdInstructor == null ||
                nuevoGrupo.IdInstructor == 0 ||
                !_context.Instructors.Any(i => i.Id_Instructor == nuevoGrupo.IdInstructor))
            {
                nuevoGrupo.IdInstructor = null;
            }

            // 2. Guardar grupo
            _context.Grupos.Add(nuevoGrupo);
            await _context.SaveChangesAsync(); // Aquí se genera el ClaveGrupo automáticamente

            // 3. Obtener el ClaveGrupo ya generado
            int claveGrupoGenerado = nuevoGrupo.ClaveGrupo;

            // 4. Crear registro en Empleadocurso
            var empleadocurso = new Empleadocurso
            {
                IdEmpleado = nuevoGrupo.IdEmpleado ?? 0, // Asegúrate que sea no nulo
                ClaveGrupo = claveGrupoGenerado,
                EstatusCurso = "Pendiente",
                Calificacion = nuevoGrupo.Calificacion
            };

            _context.Empleadocursos.Add(empleadocurso);
            await _context.SaveChangesAsync();

            // 5. Redirigir
            return RedirectToAction("Index", new { empleadoId = nuevoGrupo.IdEmpleado });
        }





        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Grupos == null)
            {
                return NotFound();
            }

            var grupo = await _context.Grupos
                .Include(g => g.IdCursoNavigation)
                .Include(g => g.IdEmpleadoNavigation)
                .Include(g => g.IdInstructorNavigation) // <-- ESTA LÍNEA ES CLAVE
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
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "Nombre");
            return View();
        }

        // POST: Grupoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClaveGrupo,IdCurso,FechaInicial,FechaFinal,Horario,Lugar,Comentarios,AreaOfrece,Status,IdEmpleado,IdInstructor,Calificacion")] Grupo grupo)
        {

                _context.Add(grupo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: Cursoes/Edit/5 
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
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "Nombre", grupo.IdEmpleado);
            ViewData["IdInstructor"] = new SelectList(_context.Instructors, "Id_Instructor", "NombreInstructor", grupo.IdInstructor);

            return View(grupo);
        }

        // POST: Grupoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClaveGrupo,IdCurso,FechaInicial,FechaFinal,Horario,Lugar,Comentarios,AreaOfrece,Status,IdEmpleado,Calificacion,IdInstructor")] Grupo grupo)
        {
            if (id != grupo.ClaveGrupo)
            {
                return NotFound();
            }

            try
            {
                // 1. Actualizar Grupo
                _context.Update(grupo);

                // 2. Buscar y actualizar Empleadocurso
                var empleadocurso = await _context.Empleadocursos
                    .FirstOrDefaultAsync(ec => ec.ClaveGrupo == grupo.ClaveGrupo);

                if (empleadocurso != null)
                {
                    empleadocurso.IdEmpleado = grupo.IdEmpleado ?? empleadocurso.IdEmpleado; // por si es null
                    empleadocurso.Calificacion = grupo.Calificacion;
                    _context.Update(empleadocurso);
                }

                // 3. Guardar cambios
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Grupos.Any(e => e.ClaveGrupo == grupo.ClaveGrupo))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Index", new { empleadoId = grupo.IdEmpleado });
        }



        // GET: Grupos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grupo = await _context.Grupos
                .Include(g => g.IdCursoNavigation)
                .Include(g => g.IdEmpleadoNavigation)
                .Include(g => g.IdInstructorNavigation)
                .FirstOrDefaultAsync(m => m.ClaveGrupo == id);

            if (grupo == null)
            {
                return NotFound();
            }

            return View(grupo); // <- esta vista debe existir: Views/Grupos/Delete.cshtml
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

        [HttpGet]
        public async Task<IActionResult> ObtenerInstructorPorCurso(int cursoId)
        {
            var curso = await _context.Cursos
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.IdCurso == cursoId);

            if (curso == null || curso.Instructor == null)
                return Json(new { success = false });

            return Json(new { success = true, idInstructor = curso.Instructor.Id_Instructor });
        }

    }
}
