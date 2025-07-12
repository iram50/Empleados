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
        public async Task<IActionResult> Index(int? empleadoId)
        {
            var model = new GruposIndexViewModel();

            // Siempre carga la lista de empleados y cursos disponibles
            model.Empleados = await _context.Empleados.ToListAsync();
            model.CursosDisponibles = await _context.Cursos
                .Include(c => c.Instructor)
                .ToListAsync();


            if (empleadoId != null && empleadoId != 0)
            {
                model.EmpleadoSeleccionadoId = empleadoId.Value;

                // Obtener empleado seleccionado
                var empleado = await _context.Empleados.FindAsync(empleadoId.Value);
                if (empleado != null)
                {
                    model.NombreEmpleadoSeleccionado = empleado.Nombre;
                }

                // Cargar cursos inscritos para el empleado seleccionado
                model.CursosInscritos = await _context.Grupos
                    .Where(g => g.IdEmpleado == empleadoId.Value)
                    .Include(g => g.IdCursoNavigation)
                    .Include(g => g.IdInstructorNavigation)
                    .ToListAsync();
            }
            else
            {
                // Si no hay empleado seleccionado, inicializa listas vacías
                model.CursosInscritos = new List<Grupo>();
                model.NombreEmpleadoSeleccionado = string.Empty;
            }

            return View(model);
        }



        // POST: Grupoes/Inscribir
        [HttpPost]
        public async Task<IActionResult> Inscribir(GruposIndexViewModel model)
        {
            if (model.EmpleadosIds == null || !model.EmpleadosIds.Any())
            {
                TempData["Error"] = "Debes agregar al menos un empleado.";
                return RedirectToAction("Index");
            }

            var grupoBase = model.NuevoGrupo;

            foreach (var empleadoId in model.EmpleadosIds)
            {
                var nuevoGrupo = new Grupo
                {
                    IdEmpleado = empleadoId,
                    IdCurso = grupoBase.IdCurso,
                    FechaInicial = grupoBase.FechaInicial,
                    FechaFinal = grupoBase.FechaFinal,
                    Horario = grupoBase.Horario,
                    Lugar = grupoBase.Lugar,
                    Comentarios = grupoBase.Comentarios,
                    IdInstructor = grupoBase.IdInstructor == 0 ? null : grupoBase.IdInstructor,
                    AreaOfrece = grupoBase.AreaOfrece,
                    Status = grupoBase.Status,
                    Calificacion = grupoBase.Calificacion
                };

                _context.Grupos.Add(nuevoGrupo);
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Grupos creados correctamente.";
            return RedirectToAction("Index");
        }





        // GET: Grupoes/RenderNuevoFormulario
        public IActionResult RenderNuevoFormulario(int index)
        {
            ViewBag.Empleados = _context.Empleados.ToList();
            ViewBag.CursosDisponibles = _context.Cursos.Include(c => c.Instructor).ToList();
            ViewData["Index"] = index;
            return PartialView("_FormularioInscripcionEmpleado", new Grupo());
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
                .Include(g => g.IdInstructorNavigation)
                .FirstOrDefaultAsync(m => m.ClaveGrupo == id);

            if (grupo == null)
            {
                return NotFound();
            }

            return View(grupo);
        }

        public IActionResult Create()
        {
            ViewData["IdCurso"] = new SelectList(_context.Cursos, "IdCurso", "NombreCurso");
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClaveGrupo,IdCurso,FechaInicial,FechaFinal,Horario,Lugar,Comentarios,AreaOfrece,Status,IdEmpleado,IdInstructor,Calificacion")] Grupo grupo)
        {
            _context.Add(grupo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "Nombre", grupo.IdEmpleado);
            ViewData["IdInstructor"] = new SelectList(_context.Instructors, "Id_Instructor", "NombreInstructor", grupo.IdInstructor);

            return View(grupo);
        }

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
                _context.Update(grupo);

                var empleadocurso = await _context.Empleadocursos
                    .FirstOrDefaultAsync(ec => ec.ClaveGrupo == grupo.ClaveGrupo);

                if (empleadocurso != null)
                {
                    empleadocurso.IdEmpleado = grupo.IdEmpleado ?? empleadocurso.IdEmpleado;
                    empleadocurso.Calificacion = grupo.Calificacion;
                    _context.Update(empleadocurso);
                }

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

            return View(grupo);
        }

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