using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entidades
{
    [Table("Ges_Bodegas")]
    public class Bodegas
    {
        [Key]
        [Column("Cod_Bodega")]
        public Guid Cod_Bodega { get; set; }

        [Column("Cod_Tienda")]
        public Guid Cod_Tienda { get; set; }

        [Column("Descripcion")]
        public string Descripcion { get; set; }

        [Column("Direccion")]
        public string Direccion { get; set; }

        [Column("Estado")]
        public string Estado { get; set; }

        [Column("Tipo_Bodega")]
        public string Tipo_Bodega { get; set; }

        [Column("Defecto")]
        public string Defecto { get; set; }
    }
}