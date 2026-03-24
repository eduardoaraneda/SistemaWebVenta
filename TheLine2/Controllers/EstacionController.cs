using Domain.Entidades;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TheLine2.Infrastructure.Persistence;
using Web.Models;

namespace Web.Controllers
{
    public class EstacionController : Controller
    {
        private readonly ApplicationDBContext _context;

        public EstacionController(ApplicationDBContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            // Obtenemos la lista de la base de datos
            var tiendas = _context.Tiendas.ToList();

            ViewBag.Tiendas = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(tiendas, "Cod_Tienda", "Descripcion");

            var tiendaUsuario = _context.Estaciones
                                .Where(e => e.Descripcion == Environment.MachineName)
                                .OrderBy(e => e.Cod_Tienda) 
                                .Select(e => e.Cod_Tienda)
                                .FirstOrDefault();

            var estacion = new EstacionViewModel()
            {
                Descripcion = Environment.MachineName,
                Cod_Tienda = tiendaUsuario
            };

            return View(estacion);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveEstacion(EstacionViewModel model)
        {
            try
            {
                var estacion = await _context.Estaciones
                    .FirstOrDefaultAsync(e => e.Descripcion == model.Descripcion);

                if (estacion == null)
                {
                    estacion = new Estacion { Cod_Estacion = Guid.NewGuid(), Descripcion = model.Descripcion };
                    _context.Estaciones.Add(estacion);
                }

                estacion.Cod_Tienda = model.Cod_Tienda;
                await _context.SaveChangesAsync();

                var nombreTienda = await _context.Tiendas
                    .Where(t => t.Cod_Tienda == model.Cod_Tienda)
                    .Select(t => t.Descripcion)
                    .FirstOrDefaultAsync();

                var identity = (ClaimsIdentity)User.Identity;
                var existingClaim = identity.FindFirst("Tienda");
                if (existingClaim != null) identity.RemoveClaim(existingClaim);
                identity.AddClaim(new Claim("Tienda", nombreTienda ?? "Sin Tienda"));

                // ✅ Actualiza claim "CodTienda"
                var claimCodTienda = identity.FindFirst("CodTienda");
                if (claimCodTienda != null) identity.RemoveClaim(claimCodTienda);
                identity.AddClaim(new Claim("CodTienda", model.Cod_Tienda.ToString()));

                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, new ClaimsPrincipal(identity));


                TempData["Success"] = "Estacion actualizada correctamente";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                TempData["Error"] = "Hubo un problema al guardar los cambios";
                return RedirectToAction("Index");
            }
        }
    }
}
