using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entidades
{
    [Table("Ges_BlvDetalle", Schema = "dbo")]
    public class BlvDetalle
    {
        [Key]
        public Guid Id_DetalleBoleta { get; set; }

        // Llave Foránea hacia Cabecera
        public Guid Id_Boleta { get; set; }

        public int Nro_Linea { get; set; }

        public Guid Cod_Bodega { get; set; }

        public Guid Cod_Producto { get; set; }

        [Required]
        [StringLength(250)]
        public string Descripcion { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 4)")]
        public decimal Cantidad { get; set; }

        [Column(TypeName = "decimal(18, 4)")]
        public decimal Cambios { get; set; }

        [Column(TypeName = "decimal(18, 4)")]
        public decimal NotaCredito { get; set; }

        public int Precio_Unitario { get; set; }

        [Column(TypeName = "decimal(18, 4)")]
        public decimal Descuento_Porcentaje { get; set; }

        public int Descuento_Monto { get; set; }

        public int Total { get; set; }

        [Required]
        [Column(TypeName = "char(1)")]
        public string Tipo_Movimiento { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "char(1)")]
        public string Tipo_Cambio { get; set; } = "N";

        [Required]
        [Column(TypeName = "char(3)")]
        public string Tipo_DocumentoDevolucion { get; set; } = string.Empty;

        public Guid Id_DetalleDocumentoDevolucion { get; set; }

        [Column(TypeName = "numeric(12, 2)")]
        public decimal PorcComi { get; set; }

        public int Monto_Comision { get; set; }

        public int MontoPrePar { get; set; }

        [Required]
        [StringLength(20)]
        public string Usuario_Descuento { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "char(1)")]
        public string Tipo_Descuento { get; set; } = string.Empty;

        [Column(TypeName = "numeric(12, 2)")]
        public decimal PorcComiSimu { get; set; }

        public int Monto_ComisionSimu { get; set; }

        public int MontoPreParSimu { get; set; }

        public int Despachado { get; set; }

        // Propiedad de Navegación (Relación con la Cabecera)
        [ForeignKey("Id_Boleta")]
        public virtual BlvCabecera Cabecera { get; set; } = null!;
    }
}