using Application.DTO;
using Application.ViewModel;
using Application.ViewModel.PuntoVenta;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infrastructure.Servicios
{
    public interface IRepositorioPuntoVenta
    {
        Task<(int Status, ResultadoVenta Datos)> GrabarVentaAsync(string tipoVenta, decimal monto, decimal vuelto, string usuario,
        string estacion, string codTienda, string validador, string xmlDocumento, string xmlPagos, string xmlGiftCards);
        Task<DocumentoImpresionVM> ObtenerDocumentoAsync(string tipo, Guid idDocumento);
        Task<List<VentaConsultaResultado>> ObtenerDocumentoCambio(string tipo, int numero, int accion);
        Task<RespuestaCambioDTO> ObtenerDetalleCompleto(string tipo, int numero, Guid codTienda, int lab, Guid? idBoletaManual = null);
        Task<(int status, dynamic datos)> GrabarNotaCreditoAsync(string usuario, string estacion, string codTienda, string xmlDocumento, string xmlPagos);
        Task<FolioEstacionDTO> ObtenerFolioEstacionAsync(string Estacion, int rut, string tipo);
        Task<GiftCardResponse> ObetenerDetalleGift(string codigo, string validador);
        Task<(int status, dynamic datos)> ValidaLaboratorio(int numero, string tipo, Guid tienda, string cambio, Guid doc, int lab);
        Task<(int status, ResultadoVenta Datos)> GrabaFactura(string estacion, Guid IdGuia, Guid empresa, int Folio, string fecha, int usuario);
        Task<(int status, dynamic Datos)> TraeVentaFolder(string Tipo_Docto, Guid Id_Documento);
        Task<(int status, dynamic Datos)> UrlConsultaFolder(Guid Id_Docto);
        Task<(int status, dynamic Datos)> ValidaUsuarioVenta(string usuario);
        Task<(int status, dynamic Datos)> ValidaDatosCliente(string rut);
        Task<(int status, List<ResultadoRegion> Datos)> CargaRegion(string usuario);
        Task<(int status, List<ResultadoCiudad> Datos)> CargaCiudad(string usuario);
        Task<(int status, List<ResultadoComuna> Datos)> CargaComuna(string usuario);
        Task<(int status, List<ResultadoProfesion> Datos)> CargaProfesiones();
        Task<(int status, string? message)> GuardarClienteEx1(ClienteGuardarEx1Dto c, string usuario);
    }
    public class RepositorioPuntoVenta : IRepositorioPuntoVenta
    {
        private readonly string connectionString;

        public RepositorioPuntoVenta(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<(int Status, ResultadoVenta Datos)> GrabarVentaAsync(
            string tipoVenta,
            decimal monto,
            decimal vuelto,
            string usuario,
            string estacion,
            string codTienda,
            string validador,
            string xmlDocumento,
            string xmlPagos,
            string xmlGiftCards)
        {
            using var connection = new SqlConnection(connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@Tipo_Venta", tipoVenta);
            parameters.Add("@Monto", monto);
            parameters.Add("@Vuelto", vuelto);
            parameters.Add("@Usuario", usuario);
            parameters.Add("@Estacion", estacion);
            parameters.Add("@Cod_Tienda", codTienda);
            parameters.Add("@Validador", validador);
            parameters.Add("@XmlDocumento", xmlDocumento);
            parameters.Add("@XmlPagos", xmlPagos);
            parameters.Add("@XmlGiftCards", xmlGiftCards);
            parameters.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var resultado = await connection.QueryFirstOrDefaultAsync<ResultadoVenta>(
                "GES_Ele_GrabaVentaSII_FO",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            int status = parameters.Get<int>("@Status");

            return (status, resultado);
        }
        public async Task<DocumentoImpresionVM> ObtenerDocumentoAsync(string tipo, Guid idDocumento)
        {
            using var cn = new SqlConnection(connectionString);

            var rows = (await cn.QueryAsync<dynamic>(
                "Ges_ConsultaDocumento2",
                new
                {
                    Tipo = tipo,
                    Id_Documento = idDocumento
                },
                commandType: CommandType.StoredProcedure)).ToList();

            if (!rows.Any())
                return null;

            // 🔹 Tomamos el encabezado desde la primera fila
            var first = rows.First();

            var documento = new DocumentoImpresionVM
            {
                RutEmpresa = first.RutEmpresa,
                Tipo = first.Tipo,
                Numero = first.Numero,
                NomEmpresa = first.NomEmpresa,
                Giro = first.Giro,
                DirEmpresa = first.DirEmpresa,
                Fono = first.Fono,
                CorreoEmpresa = first.CorreoEmpresa,
                Tienda = first.Tienda,
                DirTienda = first.DirTienda,
                NomCliente = first.NomCliente,
                RutCliente = first.RutCliente,
                DirCliente = first.DirCliente,
                ComCliente = first.ComCliente,
                Neto = first.Neto,
                Exento = first.Exento,
                Iva = first.Iva,
                cTotal = first.cTotal,
                Fecha = first.Fecha,
                Tipo_Documneto = first.Tipo_Documneto,
                DocReferencia = first.DocReferencia,
                NumReferencia = first.NumReferencia,
                FecReferencia = first.FecReferencia
            };

            // 🔹 Detalles (todas las filas)
            documento.Detalles = rows.Select(r => new DocumentoDetalleVM
            {
                Producto = r.Producto,
                Descripcion = r.Descripcion,
                Cantidad = r.Cantidad,
                Precio = r.Precio,
                Descuento = r.Descuento,
                dTotal = r.dTotal
            }).ToList();

            return documento;
        }

        public async Task<List<VentaConsultaResultado>> ObtenerDocumentoCambio(string tipo, int numero, int accion = 0)
        {
            using var cn = new SqlConnection(connectionString);
            var p = new DynamicParameters();
            p.Add("@Tipo_Docto", tipo);
            p.Add("@Nro_Impreso", numero);
            p.Add("@Accion", accion); // Agregado para soportar la lógica de GDV
            p.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.Output);

            // QueryAsync mapeará las columnas basándose en los nombres del SELECT del SP
            var rows = (await cn.QueryAsync<VentaConsultaResultado>(
                "GES_VT_TraeVentaConsultaTodoX2",
                p,
                commandType: CommandType.StoredProcedure)).ToList();

            return rows;
        }

        public async Task<RespuestaCambioDTO> ObtenerDetalleCompleto(string tipo, int numero, Guid codTienda, int lab, Guid? idDoc)
        {
            using var cn = new SqlConnection(connectionString);

            var p = new DynamicParameters();
            p.Add("@Tipo_Docto", tipo);
            p.Add("@Nro_Impreso", numero);
            p.Add("@Id_Docto", idDoc);
            p.Add("@Cod_TiendaDoc", codTienda);
            p.Add("@Nro_Laboratorio", lab);
            p.Add("@status", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var respuesta = new RespuestaCambioDTO();
            int status;

            using (var multi = await cn.QueryMultipleAsync(
                "GES_VT_ObtenerDocumentoParaCambio_Full",
                p,
                commandType: CommandType.StoredProcedure))
            {

                // 1️⃣ Cabecera
                respuesta.Cabecera = await multi.ReadFirstOrDefaultAsync<VentaConsultaResultado>();

                // 2️⃣ Detalle
                respuesta.Productos = (await multi.ReadAsync<DetalleProductoDTO>()).ToList();

                // 3️⃣ OUTPUT **DENTRO DEL USING**
                status = p.Get<int>("@status");
            }

            // 4️⃣ Validaciones FUERA del reader
            if (status == -1)
                throw new Exception("Laboratorio no existe.");

            if (status == -2)
                throw new Exception("Laboratorio no aprobado.");

            return respuesta.Cabecera != null ? respuesta : null;
        }


        public async Task<(int status, dynamic datos)> GrabarNotaCreditoAsync(string usuario, string estacion, string codTienda, string xmlDocumento, string xmlPagos)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Usuario", usuario);
                parameters.Add("@Estacion", estacion);
                parameters.Add("@Cod_Tienda", codTienda);
                parameters.Add("@XmlDocumento", xmlDocumento, DbType.String);
                parameters.Add("@XmlPagos", xmlPagos, DbType.String);
                parameters.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var result = await connection.QueryFirstOrDefaultAsync("GES_Ele_GrabaNotaCreditoSII_FO",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                int status = parameters.Get<int>("@Status");
                return (status, result);
            }
        }
        public async Task<FolioEstacionDTO> ObtenerFolioEstacionAsync(string estacion, int rut, string tipo)
        {
            using var connection = new SqlConnection(connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@Estacion", estacion);
            parameters.Add("@Usuario", rut);
            parameters.Add("@Tipo", tipo);
            parameters.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.Output);
            await connection.OpenAsync();

            return await connection.QueryFirstOrDefaultAsync<FolioEstacionDTO>(
                "GES_VT_BuscarFolioEcommerce",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<GiftCardResponse> ObetenerDetalleGift(string codigo, string validador)
        {
            using var connection = new SqlConnection(connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@Codigo_Barra", codigo);
            parameters.Add("@Codigo_Seguridad", validador);
            parameters.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var response = new GiftCardResponse();

            using (var multi = await connection.QueryMultipleAsync(
                "GES_VT_BuscaInformacionGiftCard_Ex1",
                parameters,
                commandType: CommandType.StoredProcedure))
            {
                // 1. Primer ResultSet: Info de la GiftCard
                response.Info = await multi.ReadFirstOrDefaultAsync<GiftCardInfo>();

                // Si no hay info, el status probablemente sea 0 o 2
                if (response.Info != null)
                {
                    // 2. Segundo ResultSet: Datos para el cambio/producto
                    response.DetalleCambio = await multi.ReadFirstOrDefaultAsync<GiftCardDetalle>();

                    // 3. Tercer ResultSet: Historial de movimientos
                    var historial = await multi.ReadAsync<GiftCardMovimiento>();
                    response.Historial = historial.ToList();
                }
            }

            // Capturar el parámetro de salida después de leer los resultados
            int status = parameters.Get<int>("@Status");

            // Opcional: Podrías guardar el status en tu objeto response si lo necesitas
            // response.Status = status; 

            return response;
        }
        public async Task<(int status, dynamic datos)> ValidaLaboratorio(int numero, string tipo, Guid tienda, string cambio, Guid doc, int lab)
        {
            using var connection = new SqlConnection(connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@Numero", numero);
            parameters.Add("@Tipo", tipo);
            parameters.Add("@Cod_TiendaDoc", tienda);
            parameters.Add("@Cambio", cambio);
            parameters.Add("@Id_Docto", doc);
            parameters.Add("@Nro_Laboratorio", lab);
            parameters.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = await connection.QueryFirstOrDefaultAsync(
                "GES_VT_BuscarDatosBoleta_Ex4",
                parameters,
                commandType: CommandType.StoredProcedure);
            int status = parameters.Get<int>("@Status");
            return (status, result);
        }
        public async Task<(int status, ResultadoVenta Datos)> GrabaFactura(string estacion, Guid IdGuia, Guid empresa, int Folio, string fecha, int usuario)
        {
            using var connection = new SqlConnection(connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@Estacion", estacion);
            parameters.Add("@IdGuia", IdGuia);
            parameters.Add("@Cod_Empresa", empresa);
            parameters.Add("@Folio", Folio);
            parameters.Add("@Fecha", fecha);
            parameters.Add("@Usuario", usuario);
            parameters.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var resultado = await connection.QueryFirstOrDefaultAsync<ResultadoVenta>(
                "Ges_VT_GeneraFcv_Ex2",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            int status = parameters.Get<int>("@Status");
            return (status, resultado);

        }
        public async Task<(int status, dynamic Datos)> TraeVentaFolder(string Tipo_Docto, Guid Id_Documento)
        {
            using var connection = new SqlConnection(connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@Tipo_Docto", Tipo_Docto);
            parameters.Add("@Id_Documento", Id_Documento);
            parameters.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = await connection.QueryFirstOrDefaultAsync(
                "Ges_Ele_XmlEnvioSII_Folder",
                parameters,
                commandType: CommandType.StoredProcedure);
            int status = parameters.Get<int>("@Status");
            return (status, result);
        }
        public async Task<(int status, dynamic Datos)> UrlConsultaFolder(Guid Id_Docto)
        {
            using var connection = new SqlConnection(connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@Id_Docto", Id_Docto);
            parameters.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = await connection.QueryFirstOrDefaultAsync(
                "GES_Ele_ConsultarUrl_FO",
                parameters,
                commandType: CommandType.StoredProcedure);
            int status = parameters.Get<int>("@Status");
            return (status, result);
        }
        public async Task<(int status, dynamic Datos)> ValidaUsuarioVenta(string usuario)
        {
            using var connection = new SqlConnection(connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@Rut_Cliente", usuario);
            parameters.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = await connection.QueryFirstOrDefaultAsync(
                "GES_CL_BuscarClienteRut",
                parameters,
                commandType: CommandType.StoredProcedure);
            int status = parameters.Get<int>("@Status");
            return (status, result);
        }
        public async Task<(int status, List<ResultadoRegion> Datos)> CargaRegion(string usuario)
        {
            await using var connection = new SqlConnection(connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var datos = (await connection.QueryAsync<ResultadoRegion>(
                "GES_PM_Cargar_Region",
                parameters,
                commandType: CommandType.StoredProcedure)).ToList();

            int status = parameters.Get<int?>("@Status") ?? 0;

            return (status, datos);
        }
        public async Task<(int status, List<ResultadoCiudad> Datos)> CargaCiudad(string usuario)
        {
            await using var connection = new SqlConnection(connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@Cod_Region", usuario);
            parameters.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var datos = (await connection.QueryAsync<ResultadoCiudad>(
                "GES_PM_Cargar_Ciudad",
                parameters,
                commandType: CommandType.StoredProcedure)).ToList();

            int status = parameters.Get<int?>("@Status") ?? 0;

            return (status, datos);
        }
        public async Task<(int status, List<ResultadoComuna> Datos)> CargaComuna(string usuario)
        {
            await using var connection = new SqlConnection(connectionString);

            var parameters = new DynamicParameters();
            parameters.Add("@Cod_Ciudad", usuario);
            parameters.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.Output);

            var datos = (await connection.QueryAsync<ResultadoComuna>(
                "GES_PM_Cargar_Comuna",
                parameters,
                commandType: CommandType.StoredProcedure)).ToList();

            int status = parameters.Get<int?>("@Status") ?? 0;

            return (status, datos);
        }
        public async Task<(int status, dynamic Datos)> ValidaDatosCliente(string rut)
        {
            using var connection = new SqlConnection(connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@Rut_Cliente", rut);
            parameters.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var result = await connection.QueryFirstOrDefaultAsync(
                "GES_CL_Cargar_Cliente",
                parameters,
                commandType: CommandType.StoredProcedure);
            int status = parameters.Get<int>("@Status");
            return (status, result);

        }
        public async Task<(int status, List<ResultadoProfesion> Datos)> CargaProfesiones()
        {
            await using var connection = new SqlConnection(connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var datos = (await connection.QueryAsync<ResultadoProfesion>(
                "GES_PM_Cargar_Profesion",
                parameters,
                commandType: CommandType.StoredProcedure)).ToList();
            int status = parameters.Get<int?>("@Status") ?? 0;
            return (status, datos);
        }
        public async Task<(int status, string? message)> GuardarClienteEx1(ClienteGuardarEx1Dto c, string usuario)
        {
            await using var connection = new SqlConnection(connectionString);

            var p = new DynamicParameters();

            p.Add("Rut_Cliente", c.Rut_Cliente, DbType.Int32);
            p.Add("Dv_Cliente", c.Dv_Cliente, DbType.StringFixedLength);

            p.Add("Cod_Cliente", c.Cod_Cliente, DbType.String);

            p.Add("Nombres", c.Nombres, DbType.String);
            p.Add("ApellidoPat", c.ApellidoPat, DbType.String);
            p.Add("ApellidoMat", c.ApellidoMat, DbType.String);
            p.Add("Giro", c.Giro, DbType.String);
            p.Add("Direccion", c.Direccion, DbType.String);
            p.Add("Numero", c.Numero, DbType.String);
            p.Add("Depto", c.Depto, DbType.String);
            p.Add("Poblacion", c.Poblacion, DbType.String);

            p.Add("Cod_Region", c.Cod_Region ?? "", DbType.String);
            p.Add("Cod_Ciudad", c.Cod_Ciudad ?? "", DbType.String);
            p.Add("Cod_Comuna", c.Cod_Comuna ?? "", DbType.String);

            p.Add("EstadoCivil", c.EstadoCivil, DbType.String);
            p.Add("Sexo", c.Sexo, DbType.StringFixedLength);

            // Si tu SP espera DateTime y mandas "", puedes cambiar a null:
            DateTime? fn = null;
            if (!string.IsNullOrWhiteSpace(c.FechaNacimiento))
                fn = DateTime.Parse(c.FechaNacimiento); // "YYYY-MM-DD"
            p.Add("FechaNacimiento", fn, DbType.DateTime);

            p.Add("FaceBook", c.FaceBook, DbType.String);
            p.Add("Publicidad", c.Publicidad, DbType.StringFixedLength);
            p.Add("Cod_Profesion", c.Cod_Profesion ?? "", DbType.String);

            p.Add("Telefono", c.Telefono, DbType.String);
            p.Add("Celular", c.Celular, DbType.String);
            p.Add("Mail", c.Mail, DbType.String);

            p.Add("Tipo_Cliente", c.Tipo_Cliente, DbType.StringFixedLength);
            p.Add("Fax", c.Fax, DbType.String);
            p.Add("Observaciones", c.Observaciones, DbType.String);

            p.Add("Estado", c.Estado, DbType.StringFixedLength);
            p.Add("Convenio", c.Convenio, DbType.StringFixedLength);

            DateTime? fcd = null;
            if (!string.IsNullOrWhiteSpace(c.FechaConvenioDesde)) fcd = DateTime.Parse(c.FechaConvenioDesde);
            DateTime? fch = null;
            if (!string.IsNullOrWhiteSpace(c.FechaConvenioHasta)) fch = DateTime.Parse(c.FechaConvenioHasta);

            p.Add("FechaConvenioDesde", fcd, DbType.DateTime);
            p.Add("FechaConvenioHasta", fch, DbType.DateTime);

            p.Add("Cod_ClienteConvenio", c.Cod_ClienteConvenio, DbType.String);

            p.Add("Usuario", usuario, DbType.String);

            p.Add("Banco", c.Banco, DbType.String);
            p.Add("TipoCuenta", c.TipoCuenta, DbType.String);
            p.Add("NroCuenta", c.NroCuenta, DbType.String);
            p.Add("RutCuenta", c.RutCuenta, DbType.String);

            p.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync("GES_CL_Guardar_ClienteEx1", p, commandType: CommandType.StoredProcedure);

            int status = p.Get<int?>("@Status") ?? -1;
            return (status, null);
        }
    }
}
