using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entidades
{
    [Table("Ges_Comuna", Schema = "dbo")]
    public class Comuna
    {
        [Key]
        public Guid Cod_Comuna { get; set; }

        public Guid Cod_Ciudad { get; set; }

        [Required]
        [StringLength(150)]
        public string Descripcion { get; set; } = string.Empty;

        [Timestamp]
        public byte[] EcoTimeStamp { get; set; } = null!;

        public int? Codigo_Talana { get; set; }

        [StringLength(10)]
        public string? Codigo_BX { get; set; }
    }
}