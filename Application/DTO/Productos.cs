namespace Application.DTO
{
    public class ProductoTiendaDto
    {
        public Guid Codigo { get; set; }
        public string Descripcion { get; set; }
        public int Stock { get; set; }
        public decimal Precio { get; set; }
        public decimal Descuento { get; set; }
        public decimal PrecioVenta { get; set; }
        public Guid CodBodega { get; set; }
    }

}
