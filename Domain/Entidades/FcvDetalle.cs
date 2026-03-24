namespace Domain.Entidades
{
    public class Ges_FcvDetalle
    {
        // Identificadores
        public Guid Id_DetalleFactura { get; set; }
        public Guid Id_Factura { get; set; } // FK
        public Guid Cod_Bodega { get; set; }
        public Guid Cod_Producto { get; set; }
        public Guid Id_DetalleDocumentoDevolucion { get; set; }

        // Datos de Línea y Producto
        public int Nro_Linea { get; set; }
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Cambios { get; set; }
        public decimal NotaCredito { get; set; }
        public int Precio_Unitario { get; set; }
        public int SubTotal { get; set; }

        // Totales de Línea
        public decimal Descuento_Porcentaje { get; set; }
        public int Descuento_Monto { get; set; }
        public int Total { get; set; }
        public decimal Neto { get; set; } // decimal(18,4) en detalle
        public decimal IVA { get; set; }  // decimal(18,4) en detalle

        // Movimientos
        public string Tipo_Movimiento { get; set; }         // char(1)
        public string Tipo_DocumentoDevolucion { get; set; } // char(3)
    }
}
