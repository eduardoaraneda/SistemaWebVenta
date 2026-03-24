using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entidades
{
    [Table("Ges_Prd_EstiloColor", Schema = "dbo")]
    public class EstiloColor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // SQL usa newid() por Default
        public Guid Cod_EstiloColor { get; set; }

        [Required]
        [StringLength(50)]
        public string Estilo_Color { get; set; } = string.Empty;

        public Guid Cod_Marca { get; set; }

        public Guid Cod_Super { get; set; }

        public Guid Cod_Clasificacion { get; set; }

        [Required]
        [StringLength(250)]
        public string Descripcion { get; set; } = string.Empty;

        public Guid Cod_CategoriaProveedor { get; set; }

        public Guid Cod_ColorProveedor { get; set; }

        public Guid Cod_Segmentacion { get; set; }

        public Guid Cod_Campaña { get; set; }

        public Guid Cod_Genero { get; set; }

        public Guid Cod_Categoria { get; set; }

        public Guid Cod_Temporada { get; set; }

        public int ListaCosto { get; set; }

        public int Costo { get; set; }

        [Required]
        [Column(TypeName = "char(1)")]
        public string Estado { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "char(1)")]
        public string Importado { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Largo { get; set; } = string.Empty;

        public int Cant_Tallas { get; set; }

        public int Cod_Familia { get; set; }

        [Timestamp]
        public byte[] EcoTimeStamp { get; set; } = null!;

        [Required]
        [Column(TypeName = "char(1)")]
        public string Publicable { get; set; } = "N";

        public DateTime Fecha_Lanzamiento { get; set; }

        [Required]
        [StringLength(3000)]
        public string DescripcionCorta { get; set; } = string.Empty;

        public Guid Cod_SubGenero { get; set; }

        public Guid Cod_SubSubCategoria { get; set; }

        public int Destacado { get; set; }

        public int Precio_Oferta { get; set; }

        [Required]
        [StringLength(4000)]
        public string Ficha_Tecnica { get; set; } = string.Empty;

        [Required]
        [StringLength(1)]
        public string Habilitado_Ecommerce { get; set; } = "N";

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Nro_Interno { get; set; }

        public int Novedad { get; set; }

        [Column(TypeName = "decimal(5, 4)")]
        public decimal Kilo_Volumen { get; set; }

        public int Orden { get; set; }

        [Required]
        [StringLength(1000)]
        public string Descripcion_Ceo { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Descripcion_Ecommerce { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "char(1)")]
        public string Historia { get; set; } = "N";

        [Required]
        [Column(TypeName = "char(1)")]
        public string HabDescuentoPersonal { get; set; } = "N";

        public int CantidadMaximoVenta { get; set; }

        public DateTime FechaTermino_Venta { get; set; }

        [Required]
        [Column(TypeName = "char(1)")]
        public string HabDescuentos { get; set; } = "N";

        [Required]
        [Column(TypeName = "char(1)")]
        public string Devoluciones { get; set; } = "S";

        [Required]
        [Column(TypeName = "char(1)")]
        public string RequiereLogin { get; set; } = "N";

        public int? NroTicket { get; set; }

        public DateTime FechaTrigger { get; set; }

        [StringLength(10)]
        public string? Hermandad { get; set; }

        public DateTime? Fecha_Dynamics { get; set; }

        public Guid? Cod_Empresa { get; set; }

        // Propiedad de navegación inversa (Opcional)
        public virtual ICollection<Productos> Productos { get; set; } = new List<Productos>();
    }
}