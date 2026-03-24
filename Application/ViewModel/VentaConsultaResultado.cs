namespace Application.ViewModel
{
    public class VentaConsultaResultado
    {
        // Coinciden con el SELECT del SP
        public string Tienda { get; set; }
        public string TD { get; set; }           // @Tipo_Docto
        public int Numero { get; set; }         // Cab.Nro_Impreso
        public decimal Total { get; set; }
        public string Est { get; set; }          // Cab.Estado
        public string FEmision { get; set; }     // Convert 120 o 105
        public string FechaHora { get; set; }
        public string Cliente { get; set; }      // Cli.Cliente
        public string RUT { get; set; }          // El RUT formateado con guión
        public string Vendedor { get; set; }     // Nombre_Usuario
        public string Cajero { get; set; }       // Nombre_Usuario
        public Guid Cod_Tienda { get; set; }
        public string TE { get; set; }           // Cab.TipoEmision
        public Guid Id_Doc { get; set; }         // Id_Boleta o Id_Guia o Id_Factura
    }
}
