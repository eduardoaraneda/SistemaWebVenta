namespace Domain.Entidades
{
    public class ConsultaDocumentoCab
    {
        public string? Tienda { get; set; }          // Tnd.Descripcion
        public string? TD { get; set; }              // @Tipo_Docto
        public int? Numero { get; set; }             // Nro_Impreso / Nro_Solicitud
        public decimal? Total { get; set; }          // Cab.Total
        public string? Est { get; set; }             // Cab.Estado
        public string? FEmision { get; set; }        // varchar(10)
        public string? FechaHora { get; set; }       // "dd-MM-yyyy HH:mm"
        public string? Cliente { get; set; }         // Cli.Cliente
        public string? RUT { get; set; }             // "Rut-Dv"
        public string? Vendedor { get; set; }        // Nombre_Usuario
        public string? Cajero { get; set; }          // Nombre_Usuario
        public Guid Cod_Tienda { get; set; }        // Cab.Cod_Tienda (o origen)
        public string? TE { get; set; }              // TipoEmision o fijo ('FCV','FVE',...)

        public Guid Id_Doc { get; set; }            // Id_Boleta / Id_Factura / Id_GuiaDespacho / etc.

        // Solo aparece en GDV (cuando @Tipo_Docto = 'GDV')
        public string? Electronica { get; set; }
    }
}
