namespace Domain.Entidades
{
    public class Ges_GdvCabecera
    {
        // Identificadores y Relaciones
        public Guid Id_GuiaDespacho { get; set; }
        public Guid Cod_Cliente { get; set; }
        public Guid Cod_Tienda { get; set; }
        public Guid Estacion { get; set; }
        public Guid Cod_TiendaEntrega { get; set; }
        public Guid Cod_Transportista { get; set; }
        public Guid Cod_Bodega { get; set; }
        public Guid Id_NotaCreditoProveedor { get; set; }
        public Guid Cod_TiendaDestino { get; set; }
        public Guid Cod_MotivoDes { get; set; }
        public Guid Cod_BodegaDes { get; set; }
        public Guid? ID_Factura { get; set; } // Nullable por el script
        public Guid Cod_Empresa { get; set; }
        public Guid Id_DocumentoSolicitud { get; set; }

        // Fechas
        public DateTime Fecha_Emision { get; set; }
        public DateTime Fecha_Salida { get; set; }
        public DateTime Fecha_Entrega { get; set; }
        public DateTime Fecha_Recepcion { get; set; }
        public DateTime Fecha_Registro { get; set; }
        public DateTime Fecha_Recep { get; set; }
        public DateTime? FechaRV { get; set; }

        // Valores Numéricos
        public int Neto { get; set; }
        public int Iva { get; set; }
        public int Total { get; set; }
        public int Nro_Impreso { get; set; }
        public decimal Peso { get; set; }
        public decimal Bultos { get; set; }
        public int GiftMonto { get; set; }

        // Strings y Estados
        public string Usuario_Vendedor { get; set; }
        public string Usuario { get; set; }
        public string Estado { get; set; } // char(1)
        public string Tipo_Guia { get; set; } // char(1)
        public string Tipo_DocumentoSolicitud { get; set; }
        public string Cod_TipoTransporte { get; set; }
        public string Observaciones { get; set; }
        public string Cod_Envio { get; set; }
        public string Facturada { get; set; }
        public string Usu_Recep { get; set; }
        public string Est_Recep { get; set; }
        public string TipoEmision { get; set; }
        public string RV { get; set; }
        public string UsuarioRV { get; set; }
        public string Electronica { get; set; }
        public string CargadaSii { get; set; }
        public string Ted { get; set; }

        // Propiedad de Navegación para el Detalle
        public List<Ges_GdvDetalle> Detalles { get; set; } = new List<Ges_GdvDetalle>();
    }
}
