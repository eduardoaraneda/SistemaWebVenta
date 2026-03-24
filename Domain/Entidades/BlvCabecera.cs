using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entidades
{
    [Table("Ges_BlvCabecera", Schema = "dbo")]
    public class BlvCabecera
    {
        [Key]
        public Guid Id_Boleta { get; set; }

        public Guid Cod_Cliente { get; set; }

        public DateTime Fecha_Emision { get; set; }

        public int Neto { get; set; }

        public int Iva { get; set; }

        public int Total { get; set; }

        public int Nro_Impreso { get; set; }

        [Required]
        [StringLength(20)]
        public string Usuario_Vendedor { get; set; } = string.Empty;

        public Guid Cod_Tienda { get; set; }

        public Guid Estacion { get; set; }

        [Required]
        [StringLength(20)]
        public string Usuario { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "char(1)")]
        public string Estado { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "char(1)")]
        public string Retiro_Posterior { get; set; } = string.Empty;

        public DateTime Fecha_Registro { get; set; }

        public DateTime FechaHora_Control { get; set; }

        [Required]
        [StringLength(3)]
        public string TipoEmision { get; set; } = "BLV";

        [StringLength(25)]
        public string? Validador { get; set; }

        public Guid? Cod_Empresa { get; set; }

        public int GiftMonto { get; set; }

        [Required]
        [StringLength(1)]
        public string EstadoSII { get; set; } = string.Empty;

        public DateTime? FechaEnvioSII { get; set; }

        [Required]
        [StringLength(1)]
        public string EstadoDyn { get; set; } = "P";

        public DateTime? FechaEnvioDyn { get; set; }
    }
}