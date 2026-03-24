namespace Application.ViewModel
{
    public class ConsultaCabeceraDoc
    {
        public string? Tienda { get; set; }
        public string? TD { get; set; }
        public int? Numero { get; set; }
        public decimal? Total { get; set; }

        // A veces viene como "Estado" y en BLV viene "Cab.Estado" sin alias (igual llega como "Estado" o "Estado"?)
        // Para evitar problemas, usa un alias fijo en el SP o mapea manual.
        public string? Estado { get; set; }

        public string? FEmision { get; set; }     // varchar(10)
        public string? FechaHora { get; set; }    // varchar "dd-MM-yyyy HH:mm"

        public string? Cliente { get; set; }
        public string? RUT { get; set; }

        public string? Vendedor { get; set; }
        public string? Cajero { get; set; }

        public Guid? Cod_Tienda { get; set; }

        // Campos extra (BLV/BLE/VCH/OPR/ECO)
        public string? DatoWeb { get; set; }
        public Guid? Cod_Cliente { get; set; }
        public string? Usu_Ven { get; set; }
        public int? NroSII { get; set; }
        public string? TEle { get; set; }
        public int? ParMinImp { get; set; }

        public Guid? Id_Boleta { get; set; }
        public Guid? Cod_Empresa { get; set; }
        public string? Origen { get; set; }
        public int? Dias { get; set; }

        // Campos extra GDV/GRC/XML/SOL/FCV/FVE/NCV
        public string? TG { get; set; }               // Tipo guía / tipo doc ("R","X","S")
        public string? Tienda_Destino { get; set; }
        public DateTime? Fecha_Recep { get; set; }
        public string? UsuRec { get; set; }
        public string? Nom_Rec { get; set; }
        public string? DocRel { get; set; }
        public string? Observaciones { get; set; }
        public string? BodegaOri { get; set; }
    }
}
