using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CFE.Models;
using CFE.ViewModels;
using CFE.Services;

namespace CFE.Controllers
{
    public class GrupoesController : Controller
    {
        private readonly empresaContext _context;
        private readonly ConstanciaService _constanciaService;

        public GrupoesController(empresaContext context, ConstanciaService constanciaService)
        {
            _context = context;
            _constanciaService = constanciaService;
        }

        // GET: Grupoes
        public async Task<IActionResult> Index(int? empleadoId)
        {
            var model = new GruposIndexViewModel();

            model.Empleados = await _context.Empleados.ToListAsync();
            model.CursosDisponibles = await _context.Cursos
                .Include(c => c.Instructor)
                .ToListAsync();

            if (empleadoId != null && empleadoId != 0)
            {
                model.EmpleadoSeleccionadoId = empleadoId.Value;

                var empleado = await _context.Empleados.FindAsync(empleadoId.Value);
                if (empleado != null)
                {
                    model.NombreEmpleadoSeleccionado = empleado.Nombre;
                }

                model.CursosInscritos = await _context.Grupos
                    .Where(g => g.IdEmpleado == empleadoId.Value)
                    .Include(g => g.IdCursoNavigation)
                    .Include(g => g.IdInstructorNavigation)
                    .ToListAsync();
            }
            else
            {
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
            int? redirectToEmpleadoId = null;

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

                if (redirectToEmpleadoId == null)
                {
                    redirectToEmpleadoId = empleadoId;
                }
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Grupos creados correctamente.";
            return RedirectToAction("Index", new { empleadoId = redirectToEmpleadoId });
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
                return Problem("Entity set 'empresaContext.Grupos' is null.");
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

        // MÉTODO GENERAR CONSTANCIA
        [HttpGet]
        public async Task<IActionResult> GenerarConstancia(int idEmpleado, int claveGrupo)
        {
            // Cargar el objeto Grupo directamente, incluyendo todas sus navegaciones necesarias
            var grupo = await _context.Grupos
                .Include(g => g.IdEmpleadoNavigation) // Incluye el empleado asociado a este grupo
                .Include(g => g.IdCursoNavigation)    // Incluye el curso asociado a este grupo
                .Include(g => g.IdInstructorNavigation) // Incluye el instructor asociado a este grupo
                .FirstOrDefaultAsync(g => g.IdEmpleado == idEmpleado && g.ClaveGrupo == claveGrupo);

            // Verificar si se encontró el registro del grupo
            if (grupo == null)
            {
                return NotFound("No se encontró el registro de inscripción del empleado al grupo.");
            }

            // Verificar la calificación mínima para generar la constancia desde el objeto Grupo
            if (grupo.Calificacion < 59)
            {
                return BadRequest("El empleado no alcanzó la calificación mínima para generar constancia.");
            }

            // Llama a tu servicio de constancias inyectado, pasándole el objeto Grupo
            var (pdfBytes, fileName) = _constanciaService.GenerarConstancia(grupo); // <<-- ¡AHORA PASA EL OBJETO GRUPO!

            // Devuelve el archivo PDF con el nombre generado dinámicamente
            return File(pdfBytes, "application/pdf", fileName);
        }
    }
}