﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CFE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System;

namespace CFE.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsuariosController : Controller
    {
        private readonly empresaContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UsuariosController(empresaContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // Mostrar lista de usuarios
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios.Include(u => u.Rol).ToListAsync();
            return View(usuarios);
        }

        // Mostrar formulario para crear nuevo usuario
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = _context.Roles.ToList();
            return View();
        }

        // Procesar creación del usuario
        [HttpPost]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Roles = _context.Roles.ToList();
            return View(usuario);
        }

        // Mostrar formulario para editar usuario
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            var roles = await _context.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "Id_Rol", "Nombre_Rol", usuario.RolId);

            return View(usuario);
        }

        // Procesar edición del usuario
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var usuarioExistente = await _context.Usuarios.FindAsync(usuario.Id_usuario);
                if (usuarioExistente == null)
                {
                    return NotFound();
                }

                usuarioExistente.Nombre = usuario.Nombre;
                usuarioExistente.UsuarioNombre = usuario.UsuarioNombre;
                usuarioExistente.RolId = usuario.RolId;

                // Solo actualizar la contraseña si el campo tiene valor
                if (!string.IsNullOrWhiteSpace(usuario.Clave))
                {
                    usuarioExistente.Clave = usuario.Clave;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Roles = new SelectList(_context.Roles.ToList(), "IdRol", "NombreRol", usuario.RolId);
            return View(usuario);
        }

        // Mostrar confirmación de borrado
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id_usuario == id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        // Procesar borrado
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult CambiarLogo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CambiarLogo(IFormFile nuevoLogo)
        {
            if (nuevoLogo != null && nuevoLogo.Length > 0)
            {
                var nombreArchivoNuevo = "logo-" + DateTime.Now.Ticks + Path.GetExtension(nuevoLogo.FileName);

                // ¡¡¡CORRECCIÓN CLAVE AQUÍ!!!
                // Usar _webHostEnvironment.WebRootPath para la ruta correcta a wwwroot
                var carpeta = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                var rutaNueva = Path.Combine(carpeta, nombreArchivoNuevo);

                using (var stream = new FileStream(rutaNueva, FileMode.Create))
                {
                    await nuevoLogo.CopyToAsync(stream);
                }

                var config = await _context.Configuracion.FirstOrDefaultAsync();
                if (config != null)
                {
                    // Elimina el logo anterior si no es uno por defecto
                    if (!string.IsNullOrEmpty(config.NombreLogo) && !config.NombreLogo.StartsWith("logo-default", StringComparison.OrdinalIgnoreCase) && System.IO.File.Exists(Path.Combine(carpeta, config.NombreLogo)))
                    {
                        System.IO.File.Delete(Path.Combine(carpeta, config.NombreLogo));
                        Console.WriteLine($"[UsuariosController] Eliminado logo anterior: {config.NombreLogo}"); // Depuración
                    }

                    // Actualiza el nombre del logo en la base de datos
                    config.NombreLogo = nombreArchivoNuevo;
                    _context.Update(config);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"[UsuariosController] Nuevo logo guardado en DB: {nombreArchivoNuevo}"); // Depuración
                }
                else
                {
                    // Si no hay ninguna configuración, crea una nueva entrada
                    var nuevaConfig = new Configuracion { NombreLogo = nombreArchivoNuevo };
                    _context.Configuracion.Add(nuevaConfig);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"[UsuariosController] Nueva configuración creada con logo: {nombreArchivoNuevo}"); // Depuración
                }
            }
            else
            {
                Console.WriteLine("[UsuariosController] No se seleccionó ningún archivo de logo o el archivo está vacío."); // Depuración
            }

            return RedirectToAction("CambiarLogo");
        }
    }
}