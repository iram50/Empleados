using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CFE.Models;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using TuProyecto.Models;

namespace CFE.Controllers
{
    [Authorize]
    public class EmpleadoesController : Controller
    {
        private readonly empresaContext _context;

        public EmpleadoesController(empresaContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Moder,User")]
        public async Task<IActionResult> Index(string? nombre, int? idArea, int? idPuesto, bool? empleadoActivo)
        {
            var empleados = _context.Empleados
                .Include(e => e.IdAreaNavigation)
                .Include(e => e.IdPuestoNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
            {
                empleados = empleados.Where(e => e.Nombre.Contains(nombre) || e.ApellidoPaterno.Contains(nombre));
            }

            if (idArea.HasValue)
            {
                empleados = empleados.Where(e => e.IdArea == idArea);
            }

            if (idPuesto.HasValue)
            {
                empleados = empleados.Where(e => e.IdPuesto == idPuesto);
            }

            if (empleadoActivo.HasValue)
            {
                empleados = empleados.Where(e => e.EmpleadoActivo == empleadoActivo);
            }

            ViewData["IdArea"] = new SelectList(_context.Areas, "IdAreas", "DescripcionArea");
            ViewData["IdPuesto"] = new SelectList(_context.Puestos, "IdPuesto", "DescripcionPuesto");

            return View(await empleados.ToListAsync());
        }

        [Authorize(Roles = "Admin,Moder,User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Empleados == null)
                return NotFound();

            var empleado = await _context.Empleados
                .Include(e => e.IdAreaNavigation)
                .Include(e => e.IdPuestoNavigation)
                .Include(e => e.Documentos)
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);

            if (empleado == null)
                return NotFound();

            return View(empleado);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["IdArea"] = new SelectList(_context.Areas, "IdAreas", "DescripcionArea");
            ViewData["IdPuesto"] = new SelectList(_context.Puestos, "IdPuesto", "DescripcionPuesto");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Empleado empleado, IFormFile FotoArchivo, List<IFormFile> Documentos)
        {
            if (ModelState.IsValid)
            {
                // Guardar foto
                if (FotoArchivo != null && FotoArchivo.Length > 0)
                {
                    using var ms = new MemoryStream();
                    await FotoArchivo.CopyToAsync(ms);
                    empleado.Foto = ms.ToArray();
                }

                // Guardar empleado primero
                _context.Add(empleado);
                await _context.SaveChangesAsync();

                // Guardar documentos
                if (Documentos != null && Documentos.Count > 0)
                {
                    foreach (var archivo in Documentos)
                    {
                        using var ms = new MemoryStream();
                        await archivo.CopyToAsync(ms);

                        var doc = new Documento
                        {
                            EmpleadoId = empleado.IdEmpleado,
                            Nombre = Path.GetFileNameWithoutExtension(archivo.FileName),
                            Tipo = "PDF",
                            Archivo = ms.ToArray(),
                            FechaSubida = DateTime.Now
                        };

                        _context.Documentos.Add(doc);
                    }

                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["IdArea"] = new SelectList(_context.Areas, "IdAreas", "DescripcionArea", empleado.IdArea);
            ViewData["IdPuesto"] = new SelectList(_context.Puestos, "IdPuesto", "DescripcionPuesto", empleado.IdPuesto);
            return View(empleado);
        }


        [HttpGet]
        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .Include(e => e.Documentos) 
                .FirstOrDefaultAsync(e => e.IdEmpleado == id);

            if (empleado == null)
            {
                return NotFound();
            }

            ViewData["IdArea"] = new SelectList(_context.Areas, "IdAreas", "DescripcionArea", empleado.IdArea);
            ViewData["IdPuesto"] = new SelectList(_context.Puestos, "IdPuesto", "DescripcionPuesto", empleado.IdPuesto);
            return View(empleado);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Edit(int id, Empleado empleado, IFormFile? FotoArchivo, List<IFormFile> DocumentosSubidos)

        {
            if (id != empleado.IdEmpleado) return NotFound();

            var empleadoExistente = await _context.Empleados
                .Include(e => e.Documentos)
                .FirstOrDefaultAsync(e => e.IdEmpleado == id);

            if (empleadoExistente == null) return NotFound();

            // Actualizar foto
            if (FotoArchivo != null && FotoArchivo.Length > 0)
            {
                using var ms = new MemoryStream();
                await FotoArchivo.CopyToAsync(ms);
                empleadoExistente.Foto = ms.ToArray();
            }

            // Actualizar campos
            empleadoExistente.Rfc = empleado.Rfc;
            empleadoExistente.Curp = empleado.Curp;
            empleadoExistente.Nss = empleado.Nss;
            empleadoExistente.ApellidoPaterno = empleado.ApellidoPaterno;
            empleadoExistente.ApellidoMaterno = empleado.ApellidoMaterno;
            empleadoExistente.Nombre = empleado.Nombre;
            empleadoExistente.IdArea = empleado.IdArea;
            empleadoExistente.IdPuesto = empleado.IdPuesto;
            empleadoExistente.Telefono = empleado.Telefono;
            empleadoExistente.CorreoElectronico = empleado.CorreoElectronico;
            empleadoExistente.tipo_contrato = empleado.tipo_contrato;
            empleadoExistente.fecha_nacimiento = empleado.fecha_nacimiento;
            empleadoExistente.ingreso_cfe = empleado.ingreso_cfe;
            empleadoExistente.rpe = empleado.rpe;
            empleadoExistente.jefe_inmediato = empleado.jefe_inmediato;
            empleadoExistente.escolaridad = empleado.escolaridad;
            empleadoExistente.residencia_especialidad = empleado.residencia_especialidad;
            empleadoExistente.EmpleadoActivo = empleado.EmpleadoActivo;

            // Agregar documentos nuevos
            if (DocumentosSubidos != null && DocumentosSubidos.Count > 0)
            {
                foreach (var archivo in DocumentosSubidos)
                {
                    using var ms = new MemoryStream();
                    await archivo.CopyToAsync(ms);

                    var nuevoDoc = new Documento
                    {
                        EmpleadoId = empleadoExistente.IdEmpleado,
                        Nombre = Path.GetFileNameWithoutExtension(archivo.FileName),
                        Tipo = "PDF",
                        Archivo = ms.ToArray(),
                        FechaSubida = DateTime.Now
                    };

                    _context.Documentos.Add(nuevoDoc);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .Include(e => e.IdAreaNavigation)
                .Include(e => e.IdPuestoNavigation)
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> Delete(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);
            if (documento == null) return NotFound();

            int empleadoId = documento.EmpleadoId;
            _context.Documentos.Remove(documento);
            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", "Empleadoes", new { id = empleadoId });
        }

        [HttpPost, ActionName("DeleteEmpleado")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moder")]
        public async Task<IActionResult> DeleteEmpleadoConfirmed(int id)
        {
            var empleado = await _context.Empleados
                .Include(e => e.Documentos)
                .FirstOrDefaultAsync(e => e.IdEmpleado == id);

            if (empleado == null)
                return NotFound();

            // Eliminar documentos relacionados si existen
            if (empleado.Documentos != null)
                _context.Documentos.RemoveRange(empleado.Documentos);

            _context.Empleados.Remove(empleado);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}