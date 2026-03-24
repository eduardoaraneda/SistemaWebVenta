using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TheLine2.Infrastructure.Persistence;
using Web.Models;

namespace Web.Controllers
{
    public class BodegasController :Controller
    {
        private readonly ApplicationDBContext _context;
        public BodegasController(ApplicationDBContext context)
        {
            _context = context;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var listaTiendas = await _context.Tiendas
                                         .OrderBy(t => t.Descripcion) 
                                         .ToListAsync();
            ViewBag.Tiendas = new SelectList(listaTiendas, "Cod_Tienda", "Descripcion");
            return View();
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetBodegasByTienda(Guid tiendaId)
        {
            try
            {
                // 1. Obtenemos las entidades de la DB
                var listaEntidades = await _context.Ges_Bodegas
                                             .Where(b => b.Cod_Tienda == tiendaId)
                                             .ToListAsync();

                // 2. Mapeamos manualmente a tu ViewModel
                var listaViewModel = listaEntidades.Select(b => new BodegasViewModel
                {
                    Cod_Bodega = b.Cod_Bodega,
                    Cod_Tienda = b.Cod_Tienda,
                    Descripcion = b.Descripcion,
                    Direccion = b.Direccion,
                    Estado = b.Estado
                }).ToList();

                // 3. Enviamos la lista de ViewModels a la parcial
                return PartialView("_ListadoBodegas", listaViewModel);
            }
            catch (Exception ex)
            {
                // Esto ayuda a depurar si el error es de base de datos
                return StatusCode(500, "Error interno: " + ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBodega(BodegasViewModel bodegasViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (bodegasViewModel.Cod_Bodega == null || bodegasViewModel.Cod_Bodega == Guid.Empty)
                    {
                        var nuevaBodega = new Domain.Entidades.Bodegas()
                        {
                            Cod_Bodega = Guid.NewGuid(),
                            Cod_Tienda = bodegasViewModel.Cod_Tienda,
                            Descripcion = bodegasViewModel.Descripcion,
                            Direccion = bodegasViewModel.Direccion,
                            Estado = bodegasViewModel.Estado,
                            Tipo_Bodega = bodegasViewModel.Tipo_Bodega,
                            Defecto = "N"
                        };
                        _context.Ges_Bodegas.Add(nuevaBodega);
                    }
                    else
                    {
                        var existente = await _context.Ges_Bodegas.FindAsync(bodegasViewModel.Cod_Bodega);
                        if (existente == null) return Json(new { success = false, message = "Bodega no encontrada" });

                        existente.Descripcion = bodegasViewModel.Descripcion;
                        existente.Direccion = bodegasViewModel.Direccion;
                        existente.Estado = bodegasViewModel.Estado;
                        existente.Tipo_Bodega = bodegasViewModel.Tipo_Bodega;

                    }

                    await _context.SaveChangesAsync();

                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Error en la base de datos: " + ex.Message });
                }
            }

            return Json(new { success = false, message = "Verifique los datos ingresados." });
        }
        [HttpGet]
        public async Task<JsonResult> GetBodegaById(Guid id)
        {
            var b = await _context.Ges_Bodegas.FindAsync(id);
            return Json(b);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteBodega(Guid id)
        {
            try
            {
                var bodega = await _context.Ges_Bodegas.FindAsync(id);

                if (bodega == null)
                {
                    return Json(new { success = false, message = "La bodega no existe." });
                }

                _context.Ges_Bodegas.Remove(bodega);
                await _context.SaveChangesAsync();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Si la bodega tiene registros asociados (ventas, stock), SQL dará error de FK
                return Json(new
                {
                    success = false,
                    message = "No se puede eliminar la bodega porque tiene datos relacionados o hubo un error de red."
                });
            }
        }
    }

}
