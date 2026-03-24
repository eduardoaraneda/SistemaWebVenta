namespace Application.DTO
{
    public class VentaRequest
    {
        public string TipoVenta { get; set; } // "BLV" o "GDV"
        public int Total { get; set; }
        public int Vuelto { get; set; }
        public string CodCliente { get; set; }
        public string RutVendedor { get; set; }
        public bool RetiroPosterior { get; set; }
        public string NroVoucher { get; set; }
        public string IdDocumentoCambio { get; set; }
        public string NumCambio { get; set; }
        public string TipoCambio { get; set; }
        public int TotalCambio { get; set; }
        public List<DetalleVenta> Detalles { get; set; }
        public List<DetalleCambio> Cambios { get; set; }
        public PagosVenta Pagos { get; set; }
        public int Folio { get; set; }
        public int montoCliente { get; set; }
        public int utilizadoCliente { get; set; }
        public string Registrado { get; set; }
    }
    public class DetalleCambio
    {
        public string CodProducto { get; set; }
        public string CodBodega { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public int Precio { get; set; }
        public int DescuentoMonto { get; set; }
        public int TotalFila { get; set; }

        // REFERENCIA AL DOCUMENTO ORIGINAL (Obligatorio para SII)
        public string IdDocOriginal { get; set; }
        public string NumDocOriginal { get; set; }
        public string TipoDocOriginal { get; set; } // Ej: "39" para Boleta
    }

    public class DetalleProducto
    {
        public string CodProducto { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public int Precio { get; set; }
        public int DescuentoMonto { get; set; }
        public int TotalFila { get; set; }
        public string IdDoc { get; set; }
    }

    public class PagosInfo
    {
        public int Efectivo { get; set; }
        public int Tarjeta { get; set; }
        public int Cambios { get; set; }
        public string Voucher { get; set; }
    }
    public class PagosVenta
    {
        public int Efectivo { get; set; }
        public int Tarjeta { get; set; }
        public int Cambios { get; set; }
        public string CodAutorizacion { get; set; }
        public string NroOper { get; set; }
        public string Cuotas { get; set; }
    }
    public class DetalleVenta
    {
        // Coincide con data-cod de la tabla
        public string CodProducto { get; set; }

        // Coincide con data-bodega de la tabla
        public string CodBodega { get; set; }

        public string Descripcion { get; set; }

        // Usamos decimal para precisión en cálculos, aunque el SP reciba int
        public decimal Cantidad { get; set; }

        public int Precio { get; set; }

        public int DescuentoMonto { get; set; }

        public int TotalFila { get; set; }

        // Estos campos son para compatibilidad con el XML extendido que usa tu VB.NET
        public string TipoMovimiento { get; set; } = "V"; // "V" de Venta
        public string TipoDevolucion { get; set; } = "N"; // "N" de No
    }
}
