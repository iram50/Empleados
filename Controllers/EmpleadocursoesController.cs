﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CFE.Models;
using CFE.Services; // ¡Asegúrate de que este 'using' esté presente!

namespace CFE.Controllers
{
    public class EmpleadocursoesController : Controller
    {
        private readonly empresaContext _context;
        private readonly ConstanciaService _constanciaService; // Declara la variable para tu servicio

        // Constructor del controlador
        // Ahora inyectamos tanto el contexto de la base de datos como tu servicio de constancias
        public EmpleadocursoesController(empresaContext context, ConstanciaService constanciaService)
        {
            _context = context;
            _constanciaService = constanciaService; // Asigna la instancia inyectada
        }

        // GET: Empleadocursoes
        public async Task<IActionResult> Index()
        {
            var empresaContext = _context.Empleadocursos.Include(e => e.ClaveGrupoNavigation).Include(e => e.IdEmpleadoNavigation);
            return View(await empresaContext.ToListAsync());
        }

        // GET: Empleadocursoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Empleadocursos == null)
            {
                return NotFound();
            }

            var empleadocurso = await _context.Empleadocursos
                .Include(e => e.ClaveGrupoNavigation)
                .Include(e => e.IdEmpleadoNavigation)
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);
            if (empleadocurso == null)
            {
                return NotFound();
            }

            return View(empleadocurso);
        }

        // GET: Empleadocursoes/Create
        public IActionResult Create()
        {
            ViewData["ClaveGrupo"] = new SelectList(_context.Grupos, "ClaveGrupo", "ClaveGrupo");
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "IdEmpleado");
            return View();
        }

        // POST: Empleadocursoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdEmpleado,ClaveGrupo,EstatusCurso")] Empleadocurso empleadocurso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empleadocurso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClaveGrupo"] = new SelectList(_context.Grupos, "ClaveGrupo", "ClaveGrupo", empleadocurso.ClaveGrupo);
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "IdEmpleado", empleadocurso.IdEmpleado);
            return View(empleadocurso);
        }

        // GET: Empleadocursoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Empleadocursos == null)
            {
                return NotFound();
            }

            var empleadocurso = await _context.Empleadocursos.FindAsync(id);
            if (empleadocurso == null)
            {
                return NotFound();
            }
            ViewData["ClaveGrupo"] = new SelectList(_context.Grupos, "ClaveGrupo", "ClaveGrupo", empleadocurso.ClaveGrupo);
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "IdEmpleado", empleadocurso.IdEmpleado);
            return View(empleadocurso);
        }

        // POST: Empleadocursoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdEmpleado,ClaveGrupo,EstatusCurso")] Empleadocurso empleadocurso)
        {
            if (id != empleadocurso.IdEmpleado)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empleadocurso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadocursoExists(empleadocurso.IdEmpleado))
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
            ViewData["ClaveGrupo"] = new SelectList(_context.Grupos, "ClaveGrupo", "ClaveGrupo", empleadocurso.ClaveGrupo);
            ViewData["IdEmpleado"] = new SelectList(_context.Empleados, "IdEmpleado", "IdEmpleado", empleadocurso.IdEmpleado);
            return View(empleadocurso);
        }

        // GET: Empleadocursoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Empleadocursos == null)
            {
                return NotFound();
            }

            var empleadocurso = await _context.Empleadocursos
                .Include(e => e.ClaveGrupoNavigation)
                .Include(e => e.IdEmpleadoNavigation)
                .FirstOrDefaultAsync(m => m.IdEmpleado == id);
            if (empleadocurso == null)
            {
                return NotFound();
            }

            return View(empleadocurso);
        }

        // POST: Empleadocursoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Empleadocursos == null)
            {
                return Problem("Entity set 'empresaContext.Empleadocursos' is null.");
            }
            var empleadocurso = await _context.Empleadocursos.FindAsync(id);
            if (empleadocurso != null)
            {
                _context.Empleadocursos.Remove(empleadocurso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadocursoExists(int id)
        {
            return (_context.Empleadocursos?.Any(e => e.IdEmpleado == id)).GetValueOrDefault();
        }
        
    }
}