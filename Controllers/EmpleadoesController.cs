﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CFE.Models;
using System.IO;
using Microsoft.AspNetCore.Http;

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
                    using (var ms = new MemoryStream())
                    {
                        await FotoArchivo.CopyToAsync(ms);
                        empleado.Foto = ms.ToArray();
                    }
                }

                _context.Add(empleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdArea"] = new SelectList(_context.Areas, "IdAreas", "DescripcionArea", empleado.IdArea);
            ViewData["IdPuesto"] = new SelectList(_context.Puestos, "IdPuesto", "DescripcionPuesto", empleado.IdPuesto);
            return View(empleado);
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
                        using (var ms = new MemoryStream())
                        {
                            await FotoArchivo.CopyToAsync(ms);
                            empleadoExistente.Foto = ms.ToArray(); // Asignar la nueva imagen
                        }
                    }

                    // Actualizar otros campos
                    empleadoExistente.Rfc = empleado.Rfc;
                    empleadoExistente.Curp = empleado.Curp;
                    empleadoExistente.Nss = empleado.Nss;
                    empleadoExistente.ApellidoPaterno = empleado.ApellidoPaterno;
                    empleadoExistente.ApellidoMaterno = empleado.ApellidoMaterno;
                    empleadoExistente.Nombre = empleado.Nombre;
                    empleadoExistente.IdArea = empleado.IdArea;
                    empleadoExistente.AreaEmpleado = empleado.AreaEmpleado;
                    empleadoExistente.IdPuesto = empleado.IdPuesto;
                    empleadoExistente.Puesto = empleado.Puesto;
                    empleadoExistente.Telefono = empleado.Telefono;
                    empleadoExistente.CorreoElectronico = empleado.CorreoElectronico;
                    empleadoExistente.EmpleadoActivo = empleado.EmpleadoActivo;

                    // Guardar cambios en la base de datos
                    _context.Update(empleadoExistente);
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