namespace Application.DTO
{
    public class ResultadoVenta
    {
        public string Resultado { get; set; }
        public Guid Id_Doc { get; set; }
        public int Nro_Impreso { get; set; }
        public Guid IdFactura {get; set; }
    }
}
