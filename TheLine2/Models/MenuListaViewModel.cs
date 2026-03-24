namespace TheLine2.Models
{
    public class MenuListaViewModel
    {
        public Guid Id { get; set; }
        public string Descripcion { get; set; }
        public string Icono { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool EsSubmenu { get; set; }
        public string NombrePadre { get; set; }
        // Solución para CS1061: agregar la propiedad faltante
        public Guid? SelectedMenuId { get; set; }
    }
}
