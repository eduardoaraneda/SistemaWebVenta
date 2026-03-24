using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Domain.Entidades
{
 

public class Ges_FcvCabecera
    {
        // Identificadores y Relaciones
        public Guid Id_Factura { get; set; }
        public Guid Cod_Cliente { get; set; }
        public Guid Cod_Tienda { get; set; }
        public Guid Estacion { get; set; }
        public Guid Id_GuiaDespacho { get; set; }
        public Guid Cod_Empresa { get; set; }
        public Guid? Id_Boleta_Cambio { get; set; } // Nullable
        public Guid? Cod_Condicion { get; set; }    // Nullable

        // Fechas
        public DateTime Fecha_Emision { get; set; }
        public DateTime Fecha_Vencimiento { get; set; }
        public DateTime? Fecha_Registro { get; set; } // Nullable
        public DateTime? Fecha_OC { get; set; }       // Nullable

        // Valores Numéricos
        public int Neto { get; set; }
        public int Iva { get; set; }
        public int Total { get; set; }
        public int Nro_Impreso { get; set; }

        // Strings y Estados
        public string Usuario_Vendedor { get; set; }
        public string Usuario { get; set; }
        public string Estado { get; set; }      // char(1)
        public string Electronica { get; set; } // char(1)
        public string Num_Orden { get; set; }
        public string Observacion { get; set; }

        // Relación de Navegación
        public List<Ges_FcvDetalle> Detalles { get; set; } = new List<Ges_FcvDetalle>();
    }
}
