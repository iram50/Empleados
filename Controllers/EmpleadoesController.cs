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
using SixLabors.ImageSharp; // Para redimensionar imágenes
using SixLabors.ImageSharp.Processing; // Para redimensionar imágenes

namespace CFE.Controllers
{
    public class EmpleadoesController : Controller
    {
        private readonly empresaContext _context;

        public EmpleadoesController(empresaContext context)
        {
            _context = context;
        }

        // GET: Empleadoes
        public async Task<IActionResult> Index()
        {
            var empresaContext = _context.Empleados.Include(e => e.IdAreaNavigation).Include(e => e.IdPuestoNavigation);
            return View(await empresaContext.ToListAsync());
        }

        // GET: Empleadoes/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Empleadoes/Create
        public IActionResult Create()
        {
            // Verifica que las tablas no estén vacías
            if (!_context.Areas.Any())
            {
                _context.Areas.Add(new Area { DescripcionArea = "Área de Prueba" });
                _context.SaveChanges();
            }

            if (!_context.Puestos.Any())
            {
                _context.Puestos.Add(new Puesto { DescripcionPuesto = "Puesto de Prueba" });
                _context.SaveChanges();
            }

            ViewData["IdArea"] = new SelectList(_context.Areas, "IdAreas", "DescripcionArea");
            ViewData["IdPuesto"] = new SelectList(_context.Puestos, "IdPuesto", "DescripcionPuesto");
            return View();
        }

        // POST: Empleadoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empleado empleado, IFormFile FotoArchivo)
        {
            if (ModelState.IsValid)
            {
                if (FotoArchivo != null && FotoArchivo.Length > 0)
                {
                    // Verifica el tamaño de la imagen (por ejemplo, 5 MB)
                    if (FotoArchivo.Length > 5 * 1024 * 1024) // 5 MB
                    {
                        ModelState.AddModelError("FotoArchivo", "La imagen no puede ser mayor a 5 MB.");
                        ViewData["IdArea"] = new SelectList(_context.Areas, "IdAreas", "DescripcionArea", empleado.IdArea);
                        ViewData["IdPuesto"] = new SelectList(_context.Puestos, "IdPuesto", "DescripcionPuesto", empleado.IdPuesto);
                        return View(empleado);
                    }

                    // Redimensiona la imagen (opcional)
                    empleado.Foto = RedimensionarImagen(FotoArchivo, 800, 600); // Redimensiona a 800x600
                }

                _context.Add(empleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdArea"] = new SelectList(_context.Areas, "IdAreas", "DescripcionArea", empleado.IdArea);
            ViewData["IdPuesto"] = new SelectList(_context.Puestos, "IdPuesto", "DescripcionPuesto", empleado.IdPuesto);
            return View(empleado);
        }

        // Método para redimensionar la imagen (opcional)
        private byte[] RedimensionarImagen(IFormFile archivo, int anchoMaximo, int altoMaximo)
        {
            using (var imagen = Image.Load(archivo.OpenReadStream()))
            {
                imagen.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(anchoMaximo, altoMaximo),
                    Mode = ResizeMode.Max
                }));

                using (var ms = new MemoryStream())
                {
                    imagen.Save(ms, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder());
                    return ms.ToArray();
                }
            }
        }

        // GET: Empleadoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Empleados == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }

            ViewData["IdArea"] = new SelectList(_context.Areas, "IdAreas", "DescripcionArea", empleado.IdArea);
            ViewData["IdPuesto"] = new SelectList(_context.Puestos, "IdPuesto", "DescripcionPuesto", empleado.IdPuesto);
            return View(empleado);
        }

        // POST: Empleadoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Empleado empleado, IFormFile FotoArchivo)
        {
            if (id != empleado.IdEmpleado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var empleadoExistente = await _context.Empleados.FindAsync(id);
                    if (empleadoExistente == null)
                    {
                        return NotFound();
                    }

                    // Si hay una nueva imagen, reemplazarla
                    if (FotoArchivo != null && FotoArchivo.Length > 0)
                    {
                        // Verifica el tamaño de la imagen (por ejemplo, 5 MB)
                        if (FotoArchivo.Length > 5 * 1024 * 1024) // 5 MB
                        {
                            ModelState.AddModelError("FotoArchivo", "La imagen no puede ser mayor a 5 MB.");
                            ViewData["IdArea"] = new SelectList(_context.Areas, "IdAreas", "DescripcionArea", empleado.IdArea);
                            ViewData["IdPuesto"] = new SelectList(_context.Puestos, "IdPuesto", "DescripcionPuesto", empleado.IdPuesto);
                            return View(empleado);
                        }

                        // Redimensiona la imagen (opcional)
                        empleadoExistente.Foto = RedimensionarImagen(FotoArchivo, 800, 600); // Redimensiona a 800x600
                    }

                    // Actualizar datos
                    _context.Entry(empleadoExistente).CurrentValues.SetValues(empleado);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.IdEmpleado))
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

            ViewData["IdArea"] = new SelectList(_context.Areas, "IdAreas", "DescripcionArea", empleado.IdArea);
            ViewData["IdPuesto"] = new SelectList(_context.Puestos, "IdPuesto", "DescripcionPuesto", empleado.IdPuesto);
            return View(empleado);
        }

        // GET: Empleadoes/Delete/5
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

        // POST: Empleadoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(int id)
        {
            return (_context.Empleados?.Any(e => e.IdEmpleado == id)).GetValueOrDefault();
        }
    }
}