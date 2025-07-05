using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CFE.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CFE.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsuariosController : Controller
    {
        private readonly empresaContext _context;

        public UsuariosController(empresaContext context)
        {
            _context = context;
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
            if (!ModelState.IsValid)
            {
                var roles = await _context.Roles.ToListAsync();
                ViewBag.Roles = new SelectList(roles, "Id_Rol", "NombreRol", usuario.RolId);
                return View(usuario);
            }

            var usuarioExistente = await _context.Usuarios.FindAsync(usuario.Id_usuario);
            if (usuarioExistente == null)
                return NotFound();

            usuarioExistente.Nombre = usuario.Nombre;
            usuarioExistente.UsuarioNombre = usuario.UsuarioNombre;
            usuarioExistente.Clave = usuario.Clave;
            usuarioExistente.RolId = usuario.RolId;

            try
            {
                _context.Update(usuarioExistente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                var roles = await _context.Roles.ToListAsync();
                ViewBag.Roles = new SelectList(roles, "Id_Rol", "NombreRol", usuario.RolId);
                ModelState.AddModelError("", "Error al actualizar el usuario.");
                return View(usuario);
            }
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
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

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
                    if (!string.IsNullOrEmpty(config.NombreLogo))
                    {
                        var rutaAnterior = Path.Combine(carpeta, config.NombreLogo);
                        if (System.IO.File.Exists(rutaAnterior) && !config.NombreLogo.StartsWith("logo-default"))
                        {
                            System.IO.File.Delete(rutaAnterior);
                        }
                    }

                    config.NombreLogo = nombreArchivoNuevo;
                    _context.Update(config);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction("CambiarLogo");
        }
    }
}
