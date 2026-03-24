using Application.DTO;
using Application.Security;
using Application.Seguridad;
using Azure.Core;
using Domain.Entidades;
using Infrastructure.Servicios;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;
using TheLine2.Infrastructure.Persistence;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http.Headers;

namespace Web.Controllers
{
    public class PuntoVentaController : Controller
    {
        private readonly ApplicationDBContext _context;
        private readonly IRepositorioPuntoVenta _repositorioPuntoVenta;
        private readonly HttpClient _http;

        public PuntoVentaController(ApplicationDBContext context, IRepositorioPuntoVenta repositorioPuntoVenta, HttpClient http)
        {
            _context = context;
            _repositorioPuntoVenta = repositorioPuntoVenta;
            _http = http;
        }
        public async Task<IActionResult> Index()
        {
            var estacion = Environment.MachineName;
            var rutStr = User.FindFirst("RutUsuario")?.Value ?? "0";
            int rut = 0;
            int.TryParse(rutStr, out rut);
            var result = await _repositorioPuntoVenta.ObtenerFolioEstacionAsync(estacion, rut, "BLV");
            var folio = new FolioEstacionDTO();
            folio.Nro_Interno = result.Nro_Interno;
            ViewBag.Folio = folio.Nro_Interno;
            return View();
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ValidaCliente(int rut)
        {
            var cliente = _context.clientes.FirstOrDefault(u => u.Rut_Cliente == rut);

            if (cliente == null)
                return Json(new { success = false, message = "El cliente no existe." });

            if (cliente.Estado != "V")
                return Json(new { success = false, message = "El cliente no está activo." });

            bool estaRegistrado = _context.usuarios.Any(u => u.Id == rut.ToString());

            return Json(new
            {
                success = true,
                cliente = new
                {
                    rut_Cliente = cliente.Rut_Cliente,
                    nombre = cliente.Cliente,
                    registrado = estaRegistrado,
                    CodCliente = cliente.Cod_Cliente
                }
            });
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ValidaUsuario(string rut, string contraseña)
        {
            var usuario = await _context.usuarios
                .FirstOrDefaultAsync(u => u.Id == rut && u.PasswordHash == contraseña);

            if (usuario == null)
                return Json(new { isValid = false, message = "Contraseña incorrecta." });

            // 1. Obtener la identidad actual
            var identity = (ClaimsIdentity)User.Identity;

            // 2. Remover el claim si ya existe para evitar duplicados
            var existingClaim = identity.FindFirst(CustomClaims.UsuarioRegistrado);
            if (existingClaim != null) identity.RemoveClaim(existingClaim);

            // 3. Agregar el nuevo claim
            identity.AddClaim(new Claim(CustomClaims.UsuarioRegistrado, "true"));

            await HttpContext.SignInAsync(
                IdentityConstants.ApplicationScheme,
                new ClaimsPrincipal(identity)
            );

            var validaVentaUsuario = await _repositorioPuntoVenta.ValidaUsuarioVenta(rut);

            var monto = validaVentaUsuario.Datos.MontoCompra;
            var utilizado = validaVentaUsuario.Datos.UtilizadoCompra;



            return Json(new
            {
                isValid = true,
                monto = monto,
                utilizado = utilizado
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ValidaVendedor(string rut, Guid cod_tienda)
        {
            var vendedor = _context.usuarios.Where(u => u.Id == rut).FirstOrDefault();
            if (vendedor != null)
            {
                if (vendedor.Cod_Tienda != cod_tienda)
                {
                    return Json(new { success = false, message = "El Vendedor no corresponde a la Tienda" });
                }
                else
                {
                    return Json(new { success = true, message = "Usuario valido", usuario = new { Nombre = vendedor.Nombre_Usuario } });
                }
            }
            else
            {
                return Json(new { success = false, message = "Verifique los datos ingresados." });
            }
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ValidaProducto(string producto, Guid codtienda, string registrado)
        {
            var productoTienda = await (from p in _context.Productos
                                        join e in _context.estiloColors on p.Cod_EstiloColor equals e.Cod_EstiloColor
                                        join s in _context.StockProductos on p.Cod_Producto equals s.Cod_Producto
                                        join b in _context.Ges_Bodegas on s.Cod_Tienda equals b.Cod_Tienda
                                        where p.EstiloColorTalla == producto && s.Cod_Tienda == codtienda && EF.Functions.Like(b.Descripcion, "%Principal%")
                                        select new ProductoTiendaDto
                                        {
                                            Codigo = p.Cod_Producto,
                                            Descripcion = e.Descripcion,
                                            Stock = s.Principal,
                                            PrecioVenta = e.Costo,
                                            Descuento = 0,
                                            Precio = e.Costo,
                                            CodBodega = b.Cod_Bodega
                                        }).FirstOrDefaultAsync();
            if (productoTienda == null)
            {
                return Json(new { success = false, message = "Producto no encontrado" });
            }
            if (productoTienda.Stock <= 0)
            {
                return Json(new { success = false, message = "Sin stock disponible para este producto" });
            }

            var usuarioRegistrado = registrado.ToString();

            if (usuarioRegistrado == "true")
            {
                productoTienda.Descuento = productoTienda.Precio * 0.30m;
                productoTienda.Precio = productoTienda.Precio - productoTienda.Descuento;
            }

            return Json(new { success = true, productoValido = productoTienda });
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ValidaGiftcard(string codigo, string validador)
        {
            var gisftCard = await _repositorioPuntoVenta.ObetenerDetalleGift(codigo, validador);
            if (gisftCard == null)
            {
                return Json(new { success = false, message = "Gift Card no encontrada." });
            }
            return Json(new { success = true, giftCard = gisftCard });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GrabarVenta([FromBody] VentaRequest request)
        {
            try
            {
                string codCajero = User.FindFirst("RutUsuario")?.Value ?? "0";
                string codTienda = User.FindFirst("CodTienda")?.Value ?? "";
                string estacion = User.FindFirst("CodEstacion")?.Value ?? "";
                string validador = new Random().Next(100000, 999999).ToString();
                string urlDoc;
                string urlFactura = null;
                string mensajeFactura = null;
                int? folioFactura = null;

                if (request.Registrado == "true")
                {
                    if (request.utilizadoCliente < (request.montoCliente + request.Total))
                    {
                        return Json(new { success = false, message = "Empleado Sobreapasa su Maximo de Compra" });
                    }
                }



                // 1. GRABAR VENTA NORMAL (Boleta/Factura)
                string xmlDocVenta = ConstruirXmlDocumento(request);
                string xmlPagosVenta = ConstruirXmlPagos(request);

                var (statusVenta, datosVenta) = await _repositorioPuntoVenta.GrabarVentaAsync(
                    request.TipoVenta, request.Total, request.Vuelto, codCajero,
                    estacion, codTienda, validador, xmlDocVenta, xmlPagosVenta, "<?xml version=\"1.0\"?><GiftCards/>"
                );

                if (statusVenta != 1) return Json(new { success = false, message = "Error al grabar venta" });

                if (request.TipoVenta == "GDV")
                {
                    var empresa = _context.Tiendas.Where(e => e.Cod_Tienda == Guid.Parse(codTienda)).FirstOrDefault();
                    string fechaHoy = DateTime.Now.ToString("yyyyMMdd");
                    var idEmisor = _context.Empresas.Where(e => e.Cod_Empresa == empresa.Cod_Empresa).Select(e => e.Id_Emisor).FirstOrDefault();
                    var rutEmpresa = _context.Empresas.Where(e => e.Cod_Empresa == empresa.Cod_Empresa).Select(e => e.Rut_Empresa).FirstOrDefault();
                    var dvEmpresa = _context.Empresas.Where(e => e.Cod_Empresa == empresa.Cod_Empresa).Select(e => e.Dv_Empresa).FirstOrDefault();

                    var factura = await _repositorioPuntoVenta.GrabaFactura(estacion, datosVenta.Id_Doc, empresa.Cod_Empresa.Value, datosVenta.Nro_Impreso, fechaHoy, Convert.ToInt32(codCajero));

                    if (factura.status != 1)
                    {
                        return Json(new { success = false, message = "Error al grabar factura" });
                    }

                    Guid idDoc = factura.Datos.IdFactura;

                    var (statusFcv, datosFcv) = await _repositorioPuntoVenta.TraeVentaFolder("FVE", idDoc);

                    if (statusFcv == 2)
                    {
                        return Json(new { success = false, message = "Documento ya fue TIMBRADO" });
                    }
                    else if (statusFcv != 1)
                    {
                        return Json(new { success = false, message = "Error al obtener datos para timbrado" });
                    }

                    var row = (IDictionary<string, object>)datosFcv;

                    string jsonEnvio = row["JSonEnvioSII"]?.ToString();

                    //var resultado = await TimbraDocumento(idEmisor, jsonEnvio);

                    //if (!resultado.Success && !resultado.YaTimbrado)
                    //{
                    //    return Json(new
                    //    {
                    //        success = false,
                    //        message = "Documento ya se encuentra timbrado"
                    //    });
                    //}

                    string pcRutSalida = rutEmpresa.ToString() + dvEmpresa;
                    urlDoc = "URL" + pcRutSalida + "";

                    //var consultafolder = await _repositorioPuntoVenta.UrlConsultaFolder(factura.Datos.IdFactura);

                    mensajeFactura = "Factura emitida Nro. " + factura.Datos.Nro_Impreso;
                    folioFactura = factura.Datos.Nro_Impreso;
                    urlFactura = urlDoc;

                }

              
                bool tieneNC = false;
                dynamic datosNCFinal = null;

                
                if (request.Cambios != null && request.Cambios.Any())
                {
                    string xmlDocNC = xmlDocVenta;
                    string xmlPagosNC = xmlPagosVenta;

                    var (statusNC, datosNC) = await _repositorioPuntoVenta.GrabarNotaCreditoAsync(
                        codCajero, estacion, codTienda, xmlDocNC, xmlPagosNC
                    );

                    if (statusNC == 1 && datosNC != null)
                    {
                        tieneNC = true;
                        datosNCFinal = datosNC;
                    }
                }

               
                return Json(new
                {
                    success = true,
                    idDoc = datosVenta.Id_Doc,
                    folio = folioFactura ?? datosVenta.Nro_Impreso,

                    // NC (lo que ya tienes)
                    tieneNC = tieneNC,
                    ncData = tieneNC ? new
                    {
                        idNC = datosNCFinal.Id_NotaCredito,
                        folioNC = datosNCFinal.Nro_Interno,
                        mensaje = datosNCFinal.Resultado
                    } : null,
                    urlFactura = urlFactura,
                    mensajeFactura = mensajeFactura

                });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        private string ConstruirXmlDocumento(VentaRequest r)
        {
            decimal neto = Math.Round(r.Total / 1.19m, 0);
            decimal iva = r.Total - neto;

            var documento = new XElement("DocumentoVenta",
                new XElement("Cabecera",
                    new XElement("Nro_Impreso", "0"),
                    new XElement("Cod_Cliente", r.CodCliente),
                    new XElement("Fecha_Emision", DateTime.Now.ToString("yyyyMMdd")),
                    new XElement("Neto", neto),
                    new XElement("Iva", iva),
                    new XElement("Total", r.Total),
                    new XElement("Usuario_Vendedor", r.RutVendedor),
                    new XElement("Retiro_Posterior", r.RetiroPosterior ? "S" : "N"),
                    new XElement("Nro_Interno", "0"),
                    new XElement("Nro_Voucher", "1"),
                    new XElement("Nro_Interno", r.Folio)
                )
            );

            int linea = 1;

            // 🔹 Detalles de venta
            foreach (var d in r.Detalles)
            {
                documento.Add(
                    new XElement("Detalle",
                        new XElement("Nro_Linea", linea++),
                        new XElement("Cod_Bodega", d.CodBodega),
                        new XElement("Cod_Producto", d.CodProducto),
                        new XElement("Descripcion", d.Descripcion),
                        new XElement("Cantidad", d.Cantidad),
                        new XElement("Precio_Unitario", d.Precio),
                        new XElement("Descuento_Porcentaje", 0),
                        new XElement("Descuento_Monto", d.DescuentoMonto),
                        new XElement("Total", d.TotalFila),
                        new XElement("Tipo_Movimiento", "V"),
                        new XElement("Tipo_Devolucion", "N"),
                        new XElement("Tipo_DocumentoDevolucion"),
                        new XElement("Id_DetalleDocumentoDevolucion", Guid.NewGuid()),
                        new XElement("Usuario_Descuento", "Empleado"),
                        new XElement("Tipo_Descuento", "E")
                    )
                );
            }

            // 🔹 Cambios / devoluciones
            if (r.Cambios != null)
            {
                foreach (var c in r.Cambios)
                {
                    string codBodega = Guid.TryParse(c.CodBodega, out _)
                        ? c.CodBodega
                        : Guid.Empty.ToString();

                    string idOriginal = Guid.TryParse(c.IdDocOriginal, out _)
                        ? c.IdDocOriginal
                        : Guid.NewGuid().ToString();

                    documento.Add(
                        new XElement("Detalle",
                            new XElement("Nro_Linea", linea++),
                            new XElement("Cod_Bodega", codBodega),
                            new XElement("Cod_Producto", c.CodProducto),
                            new XElement("Descripcion", c.Descripcion),
                            new XElement("Cantidad", c.Cantidad),
                            new XElement("Precio_Unitario", c.Precio),
                            new XElement("Descuento_Porcentaje", 0),
                            new XElement("Descuento_Monto", c.DescuentoMonto),
                            new XElement("Total", c.TotalFila),
                            new XElement("Tipo_Movimiento", "C"),
                            new XElement("Tipo_Devolucion", "C"),
                            new XElement("Tipo_DocumentoDevolucion", c.TipoDocOriginal ?? "BLE"),
                            new XElement("Id_DetalleDocumentoDevolucion", idOriginal)
                        )
                    );
                }
            }

            var doc = new XDocument(
                new XDeclaration("1.0", "iso-8859-1", null),
                documento
            );

            return doc.ToString(SaveOptions.DisableFormatting);
        }


        private string ConstruirXmlPagos(VentaRequest r)
        {
            var pagos = new XElement("MediosPago");
            if (r.Pagos.Efectivo > 0)
            {
                pagos.Add(new XElement("Detalle",
                    new XElement("Id_MedioPago", Guid.NewGuid().ToString()),
                    new XElement("TipoMedioPago", "EFE"),
                    new XElement("Numero", ""),
                    new XElement("Cuenta", ""),
                    new XElement("Cod_Banco", ""),
                    new XElement("Sucursal", ""),
                    new XElement("Verificador", ""),
                    new XElement("Monto", r.Pagos.Efectivo),
                    new XElement("Cod_Cliente", r.CodCliente),
                    new XElement("Fecha_Vencimiento", ""),
                    new XElement("Codigo_Autorizacion", ""),
                    new XElement("Cod_Tarjeta", Guid.NewGuid().ToString()),
                    new XElement("Cuotas", "0")
                ));
            }
            if (r.Pagos.Tarjeta > 0)
            {
                pagos.Add(new XElement("Detalle",
                    new XElement("Id_MedioPago", Guid.NewGuid().ToString()),
                    new XElement("TipoMedioPago", "TRJ"),
                    new XElement("Monto", r.Pagos.Tarjeta),
                    new XElement("Cod_Cliente", r.CodCliente),
                    new XElement("Codigo_Autorizacion", r.Pagos.CodAutorizacion ?? ""),
                    new XElement("NroOper", r.Pagos.NroOper ?? "0"),
                    new XElement("Cuotas", r.Pagos.Cuotas ?? "1")
                ));
            }
            if (r.Pagos.Cambios > 0)
            {
                pagos.Add(new XElement("Detalle",
                    new XElement("Id_MedioPago", Guid.NewGuid().ToString()),
                    new XElement("TipoMedioPago", "NCV"),
                    new XElement("Numero", ""),
                    new XElement("Cuenta", ""),
                    new XElement("Cod_Banco", ""),
                    new XElement("Sucursal", ""),
                    new XElement("Verificador", ""),
                    new XElement("Monto", r.Pagos.Cambios),
                    new XElement("Cod_Cliente", r.CodCliente),
                    new XElement("Fecha_Vencimiento", DateTime.Now.ToString("yyyyMMdd")),
                    new XElement("Codigo_Autorizacion", ""),
                    new XElement("Cod_Tarjeta", Guid.NewGuid().ToString()),
                    new XElement("Cuotas", "0")
                ));
            }
            return new XDocument(new XDeclaration("1.0", "iso-8859-1", null), pagos).ToString();
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Imprimir(string tipo, Guid id)
        {
            var model = await _repositorioPuntoVenta.ObtenerDocumentoAsync(tipo, id);

            var tipoDoc = model.Tipo_Documneto.ToString();

            if (model == null)
                return NotFound();


            string logo = "theline.png"; // default

            switch (model.RutEmpresa)
            {
                case "R.U.T.: EMPRESA1": // Comercializadora
                    logo = "Comercializadora.png";
                    break;

                case "R.U.T.: EMPRESA2": // 
                    logo = "ht.png";
                    break;

                case "R.U.T.: EMPRESA3": // International
                    logo = "International.png";
                    break;
            }

            model.LogoEmpresa = logo;

            if (tipo == "BLE")
            {
                return View("Boleta", model);
            }
            else
            {
                return View("NotaCredito", model);
            }
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ObtenerDocumentoCambio(string tipo, int numero, int validador, int lab, string tipoCambio)
        {
            var lista = await _repositorioPuntoVenta.ObtenerDocumentoCambio(tipo, numero, 0);

            if (lista == null || !lista.Any())
                return Json(new { success = false, message = "No se encontró el documento solicitado." });

            if (lista.Count > 1)
            {
                return Json(new
                {
                    success = true,
                    multiple = true,
                    datos = lista
                });
            }

            var docUnico = lista.First();
            return await ObtenerDetalleFinal(tipo, numero, docUnico.Id_Doc, lab, docUnico.Cod_Tienda, tipoCambio);
        }
        [HttpGet]
        public async Task<IActionResult> ObtenerDetalleFinal(string tipo, int numero, Guid idDoc, int lab, Guid codTienda, string tipoCambio)
        {
            try
            {
                var resultado = await _repositorioPuntoVenta.ObtenerDetalleCompleto(tipo, numero, codTienda, lab, idDoc);

                if (resultado == null)
                    return Json(new { success = false, message = "No existe Laboratorio o no corresponde al documento. " });

                if (resultado.Cabecera.Est == "A")
                    return Json(new { success = false, message = "Atención: El documento seleccionado está ANULADO." });

                if (tipoCambio == "PF")
                {

                    var validaLab = await _repositorioPuntoVenta.ValidaLaboratorio(numero, tipo, codTienda, "S", idDoc, lab);

                    if (validaLab.status == 0)
                    {
                        return Json(new { success = false, message = "Error Al Buscar Documento" });
                    }
                    else if (validaLab.status == -1)
                    {
                        return Json(new { success = false, message = "No Existe Laboratorio.." });
                    }
                    else if (validaLab.status == -2)
                    {
                        return Json(new { success = false, message = "Laboratorio no esta Aprobado para cambio por Falla.." });
                    }
                    else
                    {
                        return Json(new
                        {
                            success = true,
                            multiple = false,
                            cabecera = resultado.Cabecera,
                            productos = resultado.Productos
                        });
                    }
                }
                else
                {
                    return Json(new
                    {
                        success = true,
                        multiple = false,
                        cabecera = resultado.Cabecera,
                        productos = resultado.Productos
                    });
                }


            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public async Task<TimbradoResult> TimbraDocumento(int empresaId, string jsonBody, CancellationToken ct = default)
        {
            try
            {
                using var req = new HttpRequestMessage(
                    HttpMethod.Post,
                    "URL");

                // Headers EXACTOS requeridos por FolderERP
                req.Headers.Add("Empresa_ID", empresaId.ToString());
                req.Headers.Add("Usuario_ID", "Usuario");
                req.Headers.Add("Tocken", "Tocken");
                req.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

                req.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                using var resp = await _http.SendAsync(req, ct);
                var respText = await resp.Content.ReadAsStringAsync(ct);

                if (!resp.IsSuccessStatusCode)
                {
                    return new TimbradoResult
                    {
                        Success = false,
                        Error = $"FolderERP {(int)resp.StatusCode}: {respText}"
                    };
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var data = System.Text.Json.JsonSerializer.Deserialize<JsonDoctoResponse>(respText, options);

                if (data == null)
                {
                    return new TimbradoResult
                    {
                        Success = false,
                        Error = "Respuesta JSON inválida"
                    };
                }

                // 🟡 Detectar documento ya timbrado
                bool yaTimbrado =
                    !string.IsNullOrWhiteSpace(data.Mensaje) &&
                    data.Mensaje.Contains("ya existe", StringComparison.OrdinalIgnoreCase);

                bool success =
                    data.FolioDTE > 0 &&
                    !string.IsNullOrWhiteSpace(data.UrlDTE);

                return new TimbradoResult
                {
                    Success = success,
                    YaTimbrado = yaTimbrado,
                    PdfUrl = data.DTEpdf,
                    Data = data,
                    Error = success ? null : data.Mensaje
                };
            }
            catch (Exception ex)
            {
                return new TimbradoResult
                {
                    Success = false,
                    Error = ex.Message
                };
            }
        }

        public async Task<IActionResult> GetRegiones()
        {
            var (status, datos) = await _repositorioPuntoVenta.CargaRegion(null);

            if (status != 1)
                return BadRequest(new { success = false, message = "No se pudieron cargar regiones", status });

            return Json(datos.OrderBy(d => d.Descripcion)); 
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCiudades(string codRegion)
        {
            var (status, datos) = await _repositorioPuntoVenta.CargaCiudad(codRegion);
            if (status != 1)
                return BadRequest(new { success = false, message = "No se pudieron cargar comunas", status });
            return Json(datos.OrderBy(d => d.Descripcion)); 
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetComunas(string codCiudad)
        {
            var (status, datos) = await _repositorioPuntoVenta.CargaComuna(codCiudad);
            if (status != 1)
                return BadRequest(new { success = false, message = "No se pudieron cargar comunas", status });
            return Json(datos.OrderBy(d => d.Descripcion)); 
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> cargarDatosCliente(string rut)
        {
            var (status, datos) = await _repositorioPuntoVenta.ValidaDatosCliente(rut);
            if (status != 1)
                return BadRequest(new { success = false, message = "No se encontraron datos para el cliente." });
            return Json(datos);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> cargarProfesiones()
        {
            var (status, datos) = await _repositorioPuntoVenta.CargaProfesiones();
            if (status != 1)
                return BadRequest(new { success = false, message = "No se encontraron datos para el cliente." });
            return Json(datos.OrderBy(d => d.Descripcion));
        }
        [HttpPost]
        public async Task<IActionResult> GuardarClienteEx1([FromBody] ClienteGuardarEx1Dto model)
        {
            if (model == null)
                return BadRequest(new { success = false, message = "Datos inválidos." });

            model.Cod_Cliente = string.IsNullOrWhiteSpace(model.Cod_Cliente) ? null : model.Cod_Cliente;
            model.Cod_ClienteConvenio = string.IsNullOrWhiteSpace(model.Cod_ClienteConvenio) ? null : model.Cod_ClienteConvenio;
            if (string.IsNullOrWhiteSpace(model.FechaNacimiento))
            {
                model.FechaNacimiento = "1970-01-01";
            }
            model.FechaConvenioDesde = "1970-01-01";
            model.FechaConvenioHasta = "1970-01-01";
            var usuario = User?.Identity?.Name ?? ""; 
            var (status, message) = await _repositorioPuntoVenta.GuardarClienteEx1(model, usuario);

           
            bool ok = (status == 0 || status == 1);

            return ok
                ? Ok(new { success = true, message = message ?? "Cliente guardado.", status })
                : BadRequest(new { success = false, message = message ?? "No se pudo guardar.", status });
        }

    }
}
