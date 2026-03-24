using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class TiendaViewModel
    {
        public Guid? Cod_Tienda { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "La ciudad es obligatoria")]
        public string Ciudad { get; set; }

        public string Estado { get; set; } = "A";

        public string? Telefono { get; set; }

        [EmailAddress(ErrorMessage = "Email inválido")]
        public string? Mail { get; set; }

        [Required(ErrorMessage = "El código numérico es obligatorio")]
        public int Codigo_Numerico { get; set; }

        public int? Region { get; set; }

        public int? CodigoNike { get; set; }

        public string Codigo_Comercio { get; set; }

        public string? Ecommerce { get; set; } = "ECO";

        // Cambiado a bool? para que no falle si el checkbox no se marca
        public bool ValidaStock { get; set; }

        // Campos SII: Se quita Required para evitar bloqueos si el usuario no los llena todos
        public string? ComunaSII { get; set; }
        public string? CiudadSII { get; set; }
        public string? RegionSII { get; set; }
        public string? DireccionSii { get; set; }
        public int? SucursalSii { get; set; }

        public int? Talana { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una empresa")]
        public Guid? Cod_Empresa { get; set; }

        // Cambiado a bool para manejar el Switch del HTML
        public bool BoletaElec { get; set; }
    }
}