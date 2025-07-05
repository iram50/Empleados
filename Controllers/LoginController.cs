using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CFE.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace CFE.Controllers
{
    public class LoginController : Controller
    {
        private readonly empresaContext _context;

        public LoginController(empresaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string usuario, string clave)
        {
            var user = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.UsuarioNombre == usuario && u.Clave == clave);

            if (user == null)
            {
                ViewBag.Error = "Usuario o clave incorrectos";
                return View();
            }

            var claims = new List<Claim>
            {
            new Claim(ClaimTypes.Name, user.Nombre),
            new Claim(ClaimTypes.Role, user.Rol?.Nombre_Rol ?? "Usuario"),
            new Claim("RolId", user.RolId.ToString()) // <- este es nuevo
            };


            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Iniciar sesión
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Bienvenida", "Home");

        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
    }

}