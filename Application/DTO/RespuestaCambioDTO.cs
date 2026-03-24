using Application.ViewModel;

namespace Application.DTO
{
    // Clase de Respuesta Envolvente
    public class RespuestaCambioDTO
    {
        public VentaConsultaResultado Cabecera { get; set; }
        public List<DetalleProductoDTO> Productos { get; set; }

        // Propiedad calculada para lógica de negocio
        public bool RequiereAutorizacion => Cabecera != null &&
                                           (DateTime.Now - DateTime.Parse(Cabecera.FEmision)).TotalDays > 90;
        public int status { get; set; }
    }
}
