using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entidades
{
    [Table("Ges_Ciudad", Schema = "dbo")]
    public class Ciudad
    {
        [Key]
        public Guid Cod_Ciudad { get; set; }

        public Guid Cod_Region { get; set; }

        [Required]
        [StringLength(150)]
        public string Descripcion { get; set; } = string.Empty;

        [Timestamp]
        public byte[] EcoTimeStamp { get; set; } = null!;

        // Propiedad de navegación opcional hacia Comunas
        public virtual ICollection<Comuna> Comunas { get; set; } = new List<Comuna>();
    }
}