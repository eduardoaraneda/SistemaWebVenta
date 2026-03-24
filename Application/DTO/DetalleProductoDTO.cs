using Application.ViewModel;

namespace Application.DTO
{
    public class DetalleProductoDTO
    {
        // Identificadores (pueden venir de Boleta o Guía)
        public Guid? Id_DetalleBoleta { get; set; }
        public Guid? Id_DetalleGuiaDespacho { get; set; }
        public Guid Id_Boleta { get; set; }
        public Guid? Id_GuiaDespacho { get; set; }

        public int Nro_Linea { get; set; }
        public Guid Cod_Bodega { get; set; }
        public Guid Cod_Producto { get; set; }
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Cambios { get; set; }
        public int Devoluciones { get; set; } // Representa el CASE del SP (0 o 1)

        public decimal Precio_Unitario { get; set; }
        public decimal Total { get; set; }
        public string Cod_Rapido { get; set; }
        public decimal Descuento_Monto { get; set; }
        public string Tipo_Movimiento { get; set; }
        public string Codigo { get; set; } // Mapea EstiloColorTalla
        public int NotaCredito { get; set; }
        public string Aprobacion { get; set; } // ISNULL(Lab.Aprobacion,'')
    }



}
