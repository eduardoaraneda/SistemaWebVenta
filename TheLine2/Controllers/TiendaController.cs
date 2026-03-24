using Domain.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TheLine2.Infrastructure.Persistence;
using Web.Models;

namespace Web.Controllers
{
    public class TiendaController : Controller
    {
        private readonly ApplicationDBContext _context;

        public TiendaController(ApplicationDBContext context)
        {
            _context = context;
        }
        [Authorize]
        [HttpGet]
        public ActionResult Index()
        {
            var tiendas = _context.Tiendas.OrderBy(t => t.Descripcion).ToList();
            var empresas = _context.Empresas.OrderBy(e => e.Descripcion).ToList();

            var model = new GestionTiendasViewModel
            {
                // Mapeo de Tiendas
                Tiendas = tiendas.Select(t => new TiendaViewModel
                {
                    Cod_Tienda = t.Cod_Tienda,
                    Descripcion = t.Descripcion,
                    Direccion = t.Direccion,
                    Ciudad = t.Ciudad,
                    Estado = t.Estado,
                    Telefono = t.Telefono,
                    Mail = t.Mail,
                    Codigo_Numerico = t.Codigo_Numerico,
                    Region = t.Region ?? 0,
                    CodigoNike = t.CodigoNike ?? 0,
                    Codigo_Comercio = t.Codigo_Comercio,
                    Ecommerce = t.Ecommerce,
                    ValidaStock = t.ValidaStock == "S",
                    CiudadSII = t.CiudadSii,
                    ComunaSII = t.ComunaSii,
                    Talana = t.Codigo_Talana ?? 0,
                    RegionSII = t.RegionSii
                }).ToList(),

                // Mapeo de Empresas
                Empresas = empresas.Select(e => new EmpresaViewModel
                {
                    Cod_Empresa = e.Cod_Empresa,
                    Descripcion = e.Descripcion
                }).ToList()
            };

            return View(model);
        }
        [HttpPost]
        [Authorize]
        public async Task<JsonResult> SaveTienda(TiendaViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Existen errores en el formulario." });
                }

                // 1. Detectar si es Edición o Creación
                // Verificamos si Cod_Tienda tiene un valor válido (distinto de Guid vacío)
                bool esEdicion = model.Cod_Tienda != Guid.Empty && model.Cod_Tienda != null;

                var statusParam = new SqlParameter
                {
                    ParameterName = "status",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };

                // 2. Preparar parámetros base (comunes para ambos SP)
                var sqlParams = new List<SqlParameter>
        {
            new SqlParameter("@Tienda", model.Descripcion ?? ""),
            new SqlParameter("@Direccion", model.Direccion ?? ""),
            new SqlParameter("@Comuna", model.ComunaSII ?? ""),
            new SqlParameter("@Region", model.RegionSII ?? ""),
            new SqlParameter("@Ciudad", model.Ciudad ?? ""),
            new SqlParameter("@Mail", model.Mail ?? ""),
            new SqlParameter("@CodigoNumerico", model.Codigo_Numerico),
            new SqlParameter("@NumRegion", model.Region ?? 0),
            new SqlParameter("@CodEmpresa", model.Cod_Empresa?.ToString() ?? ""),
            new SqlParameter("@CodNike", model.CodigoNike?.ToString() ?? ""),
            new SqlParameter("@SucursalSII", model.SucursalSii ?? 0),
            new SqlParameter("@DireccionSII", model.DireccionSii ?? ""),
            new SqlParameter("@ComunaSII", model.ComunaSII ?? ""),
            new SqlParameter("@CiudadSII", model.CiudadSII ?? ""),
            new SqlParameter("@RegionSII", model.RegionSII ?? ""),
            new SqlParameter("@CodTalana", model.Talana ?? 0),
            new SqlParameter("@Telefono", int.TryParse(model.Telefono, out int tel) ? tel : 0),
            statusParam
        };

                string query;

                if (esEdicion)
                {
                    // --- LÓGICA DE EDICIÓN ---
                    query = "EXEC [dbo].[Ges_ActualizaTienda] @Cod_Tienda, @Tienda, @Direccion, @Comuna, @Region, @Ciudad, @Mail, @CodigoNumerico, @NumRegion, @CodEmpresa, @CodNike, @SucursalSII, @DireccionSII, @ComunaSII, @CiudadSII, @RegionSII, @CodTalana, @Telefono, @status OUTPUT";
                    sqlParams.Add(new SqlParameter("@Cod_Tienda", model.Cod_Tienda));
                }
                else
                {
                    // --- LÓGICA DE CREACIÓN ---
                    query = "EXEC [dbo].[Ges_CreaTienda] @Tienda, @Direccion, @Comuna, @Region, @Ciudad, @Mail, @CodigoNumerico, @NumRegion, @CodEmpresa, @CodNike, @SucursalSII, @DireccionSII, @ComunaSII, @CiudadSII, @RegionSII, @CodTalana, @Telefono, @status OUTPUT";
                }

                // 3. Ejecutar
                await _context.Database.ExecuteSqlRawAsync(query, sqlParams.ToArray());

                int resultStatus = (int)statusParam.Value;

                // 4. Manejo de respuestas (Incluyendo los nuevos códigos del SP de edición)
                return resultStatus switch
                {
                    1 => Json(new { success = true, message = esEdicion ? "Tienda actualizada exitosamente." : "Tienda creada exitosamente." }),
                    -1 => Json(new { success = false, message = "El nombre de la tienda no puede estar vacío." }),
                    -2 => Json(new { success = false, message = "La dirección no puede estar vacía." }),
                    -5 => Json(new { success = false, message = "El formato del correo electrónico es inválido." }),
                    -6 => Json(new { success = false, message = "Ya existe otra tienda con esa descripción." }),
                    -7 => Json(new { success = false, message = "La tienda que intenta editar no existe." }),
                    0 => Json(new { success = false, message = "Error interno en la base de datos." }),
                    _ => Json(new { success = false, message = $"Error desconocido (Código: {resultStatus})" })
                };
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error de conexión: " + ex.Message });
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetTiendaById(Guid id)
        {
            try
            {
                var tienda = await _context.Tiendas.FirstOrDefaultAsync(t => t.Cod_Tienda == id);
                if (tienda == null)
                {
                    return NotFound("Tienda no encontrada.");
                }
                var model = new TiendaViewModel
                {
                    Cod_Tienda = tienda.Cod_Tienda,
                    Descripcion = tienda.Descripcion,
                    Direccion = tienda.Direccion,
                    Ciudad = tienda.Ciudad,
                    Estado = tienda.Estado,
                    Telefono = tienda.Telefono,
                    Mail = tienda.Mail,
                    Codigo_Numerico = tienda.Codigo_Numerico,
                    Region = tienda.Region ?? 0,
                    CodigoNike = tienda.CodigoNike ?? 0,
                    Codigo_Comercio = tienda.Codigo_Comercio,
                    Ecommerce = tienda.Ecommerce,
                    ValidaStock = tienda.ValidaStock == "S",
                    CiudadSII = tienda.CiudadSii,
                    ComunaSII = tienda.ComunaSii,
                    Talana = tienda.Codigo_Talana ?? 0,
                    RegionSII = tienda.RegionSii,
                    SucursalSii = tienda.SucursalSii,
                    DireccionSii = tienda.DireccionSii
                    
                };
                return Json(new { success = true, data = model });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error de conexión: " + ex.Message });
            }
        }


    }
}
