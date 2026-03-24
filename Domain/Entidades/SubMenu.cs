namespace Domain.Entidades
{
    public class SubMenu
    {
        public Guid Id { get; set; }
        public string Descripcion { get; set; }
        public string IconoSubMenu { get; set; }
        // Usamos estos en lugar de UrlSubMenu
        public string Controller { get; set; }
        public string Action { get; set; }
        public Menu Menu { get; set; }
        public Guid MenuId { get; set; } 

    }
}
