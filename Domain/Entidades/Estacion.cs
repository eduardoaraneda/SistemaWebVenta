using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entidades
{
    [Table("Ges_Estaciones")]
    public class Estacion
    {
        [Key]
        [Column("Estacion")]
        public Guid Cod_Estacion { get; set; }
        public string? Descripcion { get; set; }
        public Guid Cod_Tienda { get; set; }
        public string GdvElectronica { get; set; }
    }
}
