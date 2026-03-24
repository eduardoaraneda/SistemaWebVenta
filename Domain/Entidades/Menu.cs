namespace Domain.Entidades
{
    public class Menu
    {
        public Guid Id { get; set; }
        public string Descripcion { get; set; }
        public string IconoMenu { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public ICollection<SubMenu> SubMenus { get; set; } = new List<SubMenu>();
    }
}
