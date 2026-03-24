using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TuProyecto.Models
{
    [Table("Ges_Prd_StockProductos", Schema = "dbo")]
    public class StockProductos
    {
        [Key]
        public Guid Id_PrdStock { get; set; }

        [Required]
        public Guid Cod_Tienda { get; set; }

        [Required]
        public Guid Cod_Producto { get; set; }

        [Required]
        public Guid Cod_EstiloColor { get; set; }

        [Required]
        public int Principal { get; set; }

        [Required]
        public int Comprometido { get; set; }

        [Required]
        public int Transito { get; set; }

        [Required]
        public int Fallado { get; set; }

        [Required]
        public int Sucio { get; set; }

        [Required]
        public int Laboratorio { get; set; }

        [Required]
        public int Merma { get; set; }

        [Required]
        public int SalaVenta { get; set; }

        [Required]
        public int Num_Tienda { get; set; }
    }
}