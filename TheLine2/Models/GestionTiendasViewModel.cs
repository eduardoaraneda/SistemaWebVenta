namespace Web.Models
{
    public class GestionTiendasViewModel
    {
        public List<Models.TiendaViewModel> Tiendas { get; set; }
        public List<EmpresaViewModel> Empresas { get; set; } // Ajusta según tu modelo de Empresa
    }

    public class EmpresaViewModel
    {
        public Guid Cod_Empresa { get; set; }
        public string Descripcion { get; set; }
    }
}
