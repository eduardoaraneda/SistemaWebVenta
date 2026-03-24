namespace Domain.Entidades
{
    public class Ges_GdvDetalle
    {
        // Identificadores
        public Guid Id_DetalleGuiaDespacho { get; set; }
        public Guid Id_GuiaDespacho { get; set; } // FK
        public Guid Cod_Bodega { get; set; }
        public Guid Cod_Producto { get; set; }
        public Guid Id_DetalleDocumentoDevolucion { get; set; }
        public Guid? ID_Solicitud { get; set; }
        public Guid? ID_DetalleSolicitud { get; set; }

        // Datos de Línea y Producto
        public int Nro_Linea { get; set; }
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Cambios { get; set; }
        public decimal Recepcionado { get; set; }
        public decimal Devoluciones { get; set; }
        public int Precio_Unitario { get; set; }

        // Descuentos y Comisiones
        public decimal Descuento_Porcentaje { get; set; }
        public int Descuento_Monto { get; set; }
        public int Total { get; set; }
        public decimal PorcComi { get; set; }
        public int Monto_Comision { get; set; }
        public int MontoPrePar { get; set; }
        public decimal PorcComiSimu { get; set; }
        public int Monto_ComisionSimu { get; set; }
        public int MontoPreParSimu { get; set; }
        public string Tipo_Descuento { get; set; }
        public string Usuario_Descuento { get; set; }

        // Estados y Ajustes
        public string Tipo_Movimiento { get; set; }
        public string Tipo_DocumentoDevolucion { get; set; }
        public string Tipo_Ajuste { get; set; }
        public string Usr_Ajuste { get; set; }
        public DateTime Fecha_Ajuste { get; set; }
        public string CCosto { get; set; }
        public string Observacion { get; set; }
        public int? Estado_Producto_Devolucion { get; set; }
    }
}
