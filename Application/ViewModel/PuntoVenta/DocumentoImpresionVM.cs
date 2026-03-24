namespace Application.ViewModel.PuntoVenta
{
    public class DocumentoImpresionVM
    {
        public string RutEmpresa { get; set; }
        public string Tipo { get; set; }
        public string Numero { get; set; }
        public string NomEmpresa { get; set; }
        public string Giro { get; set; }
        public string DirEmpresa { get; set; }
        public string Fono { get; set; }
        public string CorreoEmpresa { get; set; }

        public string Tienda { get; set; }
        public string DirTienda { get; set; }

        public string NomCliente { get; set; }
        public string RutCliente { get; set; }
        public string DirCliente { get; set; }
        public string ComCliente { get; set; }

        public string Neto { get; set; }
        public string Exento { get; set; }
        public string Iva { get; set; }
        public string cTotal { get; set; }
        public string Fecha { get; set; }

        public string Tipo_Documneto { get; set; }
        public string DocReferencia { get; set; }
        public int NumReferencia { get; set; }
        public string FecReferencia { get; set; }

        public List<DocumentoDetalleVM> Detalles { get; set; } = new();
        public string LogoEmpresa { get; set; }

    }

}
