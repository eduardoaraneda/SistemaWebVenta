namespace Application.DTO
{
    public class ConsultaDetalleDoc
    {
        public int? Lin { get; set; }
        public string? Producto { get; set; }
        public string? Talla { get; set; }
        public string? Codigo { get; set; }
        public string? Gpc { get; set; }

        public decimal? Cantidad { get; set; }
        public decimal? PUnidad { get; set; }
        public decimal? Total { get; set; }

        // BLV/FCV/FVE/NCV
        public decimal? DescValor { get; set; }
        public int? Cambios { get; set; }
        public string? TMov { get; set; }

        // BLV
        public string? NumCambio { get; set; }
        public string? FEmision { get; set; }
        public string? Tienda { get; set; }

        // IDs (varían por tipo)
        public Guid? Id_DetalleBoleta { get; set; }
        public Guid? Id_DetalleFactura { get; set; }
        public Guid? Id_DetalleNotaCredito { get; set; }

        // NCV: doc relacionado
        public string? DocRel { get; set; }

        // GDV / GRC / XML / SOL extras
        public int? Recep { get; set; }              // GDV convierte a int / GRC trae CantidadRecepcionada / XML usa función
        public string? BodegaOrigen { get; set; }
        public string? TallaProv { get; set; }       
        public int? Desp { get; set; }              
        public string? Estilo_Color { get; set; }   
    }
}
