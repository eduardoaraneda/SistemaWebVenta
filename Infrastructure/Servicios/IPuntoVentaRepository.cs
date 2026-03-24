using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using Web.Models;

namespace Infrastructure.Servicios
{
    public interface IPuntoVentaRepository
    {
        Task<DocumentoImpresionViewModel> ObtenerDocumentoImpresion(Guid idDocumento, string tipo);
    }

    public class PuntoVentaRepository : IPuntoVentaRepository
    {
        private readonly string connectionString;
        public PuntoVentaRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<DocumentoImpresionViewModel> ObtenerDocumentoImpresion(Guid idDocumento, string tipo)
        {
            using var cn = new SqlConnection(connectionString);

            using var multi = await cn.QueryMultipleAsync(
                "Ges_ConsultaDocumento",
                new { Tipo = tipo, Id_Documento = idDocumento },
                commandType: CommandType.StoredProcedure
            );

            var encabezado = await multi.ReadSingleAsync<DocumentoImpresionViewModel>();
            var detalles = (await multi.ReadAsync<DetalleDocumentoVM>()).ToList();

            encabezado.Detalles = detalles;

            return encabezado;
        }

    }
}
