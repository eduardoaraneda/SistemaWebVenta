using Application.DTO;
using Infrastructure.Servicios;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using TheLine2.Infrastructure.Persistence;

namespace Web.Controllers
{
    
    public class ConsultaDocumentosController : Controller
    {
        private readonly ApplicationDBContext context;
        private readonly IRepositorioConsultaDocumento repositorioConsultaDocumento;
        private readonly HttpClient http;

        public ConsultaDocumentosController(ApplicationDBContext context, IRepositorioConsultaDocumento repositorioConsultaDocumento, HttpClient http)
        {
            this.context = context;
            this.repositorioConsultaDocumento = repositorioConsultaDocumento;
            this.http = http;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ConsultarDocumento(string tipo, int numero)
        {
            var (status, datos) = await repositorioConsultaDocumento.ConsultaDocumento(tipo, numero);

            if (status != 1 || datos == null || datos.Count == 0)
            {
                // Recomendado para manejar en success del JS
                return Json(new { success = false, message = "No se encontraron documentos", status });
            }

            if (datos.Count > 1)
            {
                return Json(new
                {
                    success = true,
                    multiple = true,
                    datos
                });
            }

            var doc = datos.First(); // ✅ doc completo
            return await ObtenerDetalleFinal(tipo, numero, doc.Id_Doc, doc.Cod_Tienda);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerDetalleFinal(string tipo, int numero, Guid idDoc, Guid codTienda)
        {
            try
            {
                var resultado = await repositorioConsultaDocumento.ObtenerDetalleCompleto(tipo, numero, codTienda, idDoc);

                return Json(new
                {
                    success = true,
                    multiple = false,
                    cabecera = resultado.Cabecera,
                    productos = resultado.Detalle,
                    pagos = resultado.Pagos
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
    
}
