namespace Application.DTO
{
    public class GiftCardResponse
    {
        // Primer ResultSet: Información General y Activación
        public GiftCardInfo Info { get; set; }

        // Segundo ResultSet: Detalle para el proceso de Cambio/Venta
        public GiftCardDetalle DetalleCambio { get; set; }

        // Tercer ResultSet: Historial de uso/documentos asociados
        public List<GiftCardMovimiento> Historial { get; set; }
    }

    public class GiftCardInfo
    {
        public string Codigo_Barra { get; set; }
        public int Monto { get; set; }
        public int Utilizado { get; set; }
        public int Disponible { get; set; }
        public string Estado { get; set; }
        public DateTime FVence { get; set; }
        public string PLista { get; set; }
        public DateTime Fecha_Activacion { get; set; }
        public string Tipo_Documento { get; set; }
        public int Numero_Documento { get; set; }
        public Guid Cod_Tienda { get; set; }
        public string Tipo_DocumentoCambio { get; set; }
        public int Numero_DocumentoCambio { get; set; }
        public string Fecha_Cambio { get; set; } // Puede ser string o DateTime según el procedimiento
        public Guid Cod_Bodega { get; set; }
    }

    public class GiftCardDetalle
    {
        public string Tipo { get; set; }
        public string Codigo_Producto { get; set; }
        public string Descripcion { get; set; }
        public int Precio_Unitario { get; set; }
        public int Cantidad { get; set; }
        public int Descuento { get; set; }
        public int Total { get; set; }
        public Guid Cod_Bodega { get; set; }
        public Guid Cod_Producto { get; set; }
        public Guid Id_DetalleBoleta { get; set; }
        public string Codigo_Barra { get; set; }
        public string Codigo_Seguridad { get; set; }
        public Guid Id_GiftCard { get; set; }
    }

    public class GiftCardMovimiento
    {
        public string Tienda { get; set; }
        public string TD { get; set; }
        public string Numero { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
    }
}
