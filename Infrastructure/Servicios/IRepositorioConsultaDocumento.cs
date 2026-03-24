using Application.DTO;
using Application.ViewModel;
using Dapper;
using Domain.Entidades;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infrastructure.Servicios
{
    public interface IRepositorioConsultaDocumento
    {
        Task<(int status, List<ConsultaDocumentoCab> Datos)> ConsultaDocumento(string tipo, int numero);
        Task<RespuestaConsultaDoc> ObtenerDetalleCompleto(string tipo, int numero, Guid codTienda, Guid idBoletaManual);
    }

    public class RepositorioConsultaDocumento : IRepositorioConsultaDocumento
    {
        private readonly string connectionString;

        public RepositorioConsultaDocumento(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<(int status, List<ConsultaDocumentoCab> Datos)> ConsultaDocumento(string tipo, int numero)
        {
            await using var connection = new SqlConnection(connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@Tipo_Docto", tipo);
            parameters.Add("@Nro_Impreso", numero);
            parameters.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.Output);
            var datos = (await connection.QueryAsync<ConsultaDocumentoCab>(
                "GES_VT_TraeVentaConsultaTodoX2",
                parameters,
                commandType: CommandType.StoredProcedure)).ToList();
            int status = parameters.Get<int?>("@Status") ?? 0;
            return (status, datos);
        }
        public async Task<RespuestaConsultaDoc> ObtenerDetalleCompleto(string tipo, int numero, Guid codTienda, Guid idBoletaManual)
        {
            await using var connection = new SqlConnection(connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@Tipo_Docto", tipo);
            parameters.Add("@Tipo_Emi", tipo);
            parameters.Add("@Nro_Impreso", numero);
            parameters.Add("@Cod_Tienda", codTienda);
            parameters.Add("@Id_Docto", idBoletaManual);
            parameters.Add("@Status", dbType: DbType.Int32, direction: ParameterDirection.Output);
            using var multi = await connection.QueryMultipleAsync(
                "GES_VT_TraeVentaConsulta_Ex3",
                parameters,
                commandType: CommandType.StoredProcedure);
            var respuesta = new RespuestaConsultaDoc
            {
                Cabecera = await multi.ReadFirstOrDefaultAsync<ConsultaCabeceraDoc>(),
                Detalle = (await multi.ReadAsync<ConsultaDetalleDoc>()).ToList(),
                Pagos = (await multi.ReadAsync<ConsultaPagoDoc>()).ToList(),
                Status = 1
            };
            return respuesta;

        }
    }
}
