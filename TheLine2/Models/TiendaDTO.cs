namespace Domain.DTOs
{
    public class TiendaDTO
    {
        public Guid? Cod_Tienda { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public string Ciudad { get; set; }
        public string? Telefono { get; set; }
        public string? Mail { get; set; }
        public int Codigo_Numerico { get; set; }
        public int? Region { get; set; }
        public int? CodigoNike { get; set; }
        public Guid? Cod_Empresa { get; set; }
        // ... todos los campos necesarios para el SP
        public string? ComunaSII { get; set; }
        public string? RegionSII { get; set; }
        public string? DireccionSii { get; set; }
        public string? CiudadSII { get; set; }
        public int? SucursalSii { get; set; }
        public int? Talana { get; set; }
    }
}