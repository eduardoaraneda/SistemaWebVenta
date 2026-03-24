namespace Application.DTO
{
    public class ClienteGuardarEx1Dto
    {
        public int Rut_Cliente { get; set; }
        public string Dv_Cliente { get; set; } = "";

        public string? Cod_Cliente { get; set; }

        public string Nombres { get; set; } = "";
        public string ApellidoPat { get; set; } = "";
        public string ApellidoMat { get; set; } = "";
        public string Giro { get; set; } = "";
        public string Direccion { get; set; } = "";
        public string Numero { get; set; } = "";
        public string Depto { get; set; } = "";
        public string Poblacion { get; set; } = "";

        public string? Cod_Region { get; set; }
        public string? Cod_Ciudad { get; set; }
        public string? Cod_Comuna { get; set; }

        public string EstadoCivil { get; set; } = ""; // "S"/"C"
        public string Sexo { get; set; } = "M";
        public string FechaNacimiento { get; set; } = ""; // "YYYY-MM-DD" o ""

        public string FaceBook { get; set; } = "";
        public string Publicidad { get; set; } = "N";
        public string? Cod_Profesion { get; set; }

        public string Telefono { get; set; } = "";
        public string Celular { get; set; } = "";
        public string Mail { get; set; } = "";

        public string Tipo_Cliente { get; set; } = "";
        public string Fax { get; set; } = "";
        public string Observaciones { get; set; } = "";

        public string Estado { get; set; } = "V";
        public string Convenio { get; set; } = "N";
        public string FechaConvenioDesde { get; set; } = "";
        public string FechaConvenioHasta { get; set; } = "";
        public string? Cod_ClienteConvenio { get; set; }

        public string Banco { get; set; } = "";
        public string TipoCuenta { get; set; } = "";
        public string NroCuenta { get; set; } = "";
        public string RutCuenta { get; set; } = "";
    }
}
