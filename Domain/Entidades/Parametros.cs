using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entidades
{
    [Table("Ges_Parametros", Schema = "dbo")]
    public class Parametros
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Cod_Parametro { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Cod_Numerico { get; set; }

        [Required]
        [StringLength(2)]
        public string Tipo { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        public string Valor { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "char(1)")]
        public string Estado { get; set; } = string.Empty;

        public DateTime Fecha_Creacion { get; set; }

        [Required]
        [StringLength(20)]
        public string Usuario_Creacion { get; set; } = string.Empty;

        public DateTime Fecha_Modificacion { get; set; }

        [Required]
        [StringLength(20)]
        public string Usuario_Modificacion { get; set; } = string.Empty;

        [Required]
        [StringLength(1)]
        public string TipoRel { get; set; } = string.Empty;

        public Guid? Cod_ParametroRel { get; set; }

        public DateTime? Fdesde { get; set; }

        public DateTime? FHasta { get; set; }

        public int? DiasStock { get; set; }

        public int CargoPayroll { get; set; }

        [Required]
        [StringLength(50)]
        public string DatoAdic1 { get; set; } = string.Empty;

        [Timestamp]
        public byte[] EcoTimeStamp { get; set; } = null!;

        [StringLength(500)]
        public string? Traduccion { get; set; }

        [Required]
        [Column(TypeName = "char(1)")]
        public string Ecommerce { get; set; } = "N";

        public int TipoPagoFolder { get; set; }

        [StringLength(30)]
        public string? Codigo_Talana { get; set; }

        // Propiedad de navegación opcional para la relación recursiva
        [ForeignKey("Cod_ParametroRel")]
        public virtual Parametros? ParametroRelacionado { get; set; }
    }
}