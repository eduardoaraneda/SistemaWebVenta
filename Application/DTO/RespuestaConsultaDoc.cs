using Application.ViewModel;

namespace Application.DTO
{
    public class RespuestaConsultaDoc
    {
        public ConsultaCabeceraDoc? Cabecera { get; set; }
        public List<ConsultaDetalleDoc> Detalle { get; set; } = new();
        public List<ConsultaPagoDoc> Pagos { get; set; } = new();

        public int Status { get; set; }
    }
}
