namespace Application.DTO
{
    public class ConsultaPagoDoc
    {
        public string? TPago { get; set; }
        public decimal? Monto { get; set; }

        public string? Cuo { get; set; }
        public string? Numero { get; set; }

        // según branch: Autoriza u Oper
        public string? Autoriza { get; set; }
        public string? Oper { get; set; }

        public string? Tarjeta { get; set; }
        public string? Relacionado { get; set; }
    }
}
