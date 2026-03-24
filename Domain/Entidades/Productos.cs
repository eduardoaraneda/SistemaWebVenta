using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entidades
{
    [Table("Ges_Prd_Productos", Schema = "dbo")]
    public class Productos
    {
        [Key]
        public Guid Cod_Producto { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Cod_Rapido { get; set; }

        public Guid Cod_EstiloColor { get; set; }

        public Guid Cod_Talla { get; set; }

        [Required]
        [StringLength(100)]
        public string EstiloColorTalla { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Codigo_Barra { get; set; } = string.Empty;

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

        [Timestamp] // Maneja el campo EcoTimeStamp para concurrencia
        public byte[] EcoTimeStamp { get; set; } = null!;

        [StringLength(50)]
        public string? CodInt_Proveedor { get; set; }

        public int? NroTicket { get; set; }

        public int? NroTicket1 { get; set; }

        public int? NroTicketHIT { get; set; }

        public int? NroTicket1HIT { get; set; }

        public Guid? Cod_InfoTalla { get; set; }
        // Dentro de la clase Productos:
        [ForeignKey("Cod_EstiloColor")]
        public virtual EstiloColor EstiloColor { get; set; } = null!;
    }
}