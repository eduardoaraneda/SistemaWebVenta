namespace Web.Models
{
    public class DocumentoImpresionViewModel
    {
        public Guid IdDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public string Folio { get; set; }

        public string RutEmpresa { get; set; }
        public string RazonSocial { get; set; }
        public string Giro { get; set; }
        public string Direccion { get; set; }

        public string RutCliente { get; set; }
        public string NombreCliente { get; set; }

        public DateTime Fecha { get; set; }

        public List<DetalleDocumentoVM> Detalles { get; set; }

        public decimal Total { get; set; }
        public decimal Neto { get; set; }
        public decimal IVA { get; set; }

        public string QrBase64 { get; set; }
    }

    public class DetalleDocumentoVM
    {
        public string Producto { get; set; }
        public decimal Cantidad { get; set; }
        public decimal Precio { get; set; }
        public decimal Total { get; set; }
    }
}
