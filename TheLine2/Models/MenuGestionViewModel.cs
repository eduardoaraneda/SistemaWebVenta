using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Models
{
    public class MenuGestionViewModel
    {
        public Guid? SelectedMenuId { get; set; } // Si esto tiene valor, es un Submenú
        public string Descripcion { get; set; }
        public string Icono { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }

        // Lista para llenar el select de Menús Padres
        public List<SelectListItem> MenusPadres { get; set; }
    }
}
