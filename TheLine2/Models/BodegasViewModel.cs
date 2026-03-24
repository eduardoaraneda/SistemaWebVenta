using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class BodegasViewModel
    {

        // --- Propiedades para Crear/Editar (El Formulario) ---
        public Guid? Cod_Bodega { get; set; } // Null si es nueva

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(150)]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; } = string.Empty;

        public string Estado { get; set; } = "V"; // 'A' por defecto

        public Guid Cod_Tienda { get; set; }
        public string Tipo_Bodega { get; set; } 
    }
}
