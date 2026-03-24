using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entidades
{
    [Table("Ges_Tiendas")] // Asegúrate de que el nombre coincida con la tabla en SQL
    public class Tienda
    {
        [Key]
        public Guid Cod_Tienda { get; set; }

        // Mantenemos string? para evitar el error de SqlNullValueException en cadenas
        public string? Descripcion { get; set; }
        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }
        public string? Estado { get; set; }
        public string? IndLinea { get; set; }
        public string? Telefono { get; set; }
        public string? Mail { get; set; }

        // Tipos de valor marcados como nulables para evitar el error "Data is Null"
        public int Codigo_Numerico { get; set; }
        public int? Region { get; set; }
        public string? ValidaStock { get; set; }
        public Guid? Cod_Empresa { get; set; }
        public string? ValidaCliente { get; set; }
        public int? CodigoNike { get; set; }
        public int? CodigoNike_Seq { get; set; }
        public string? Codigo_Comercio { get; set; }
        public string? ValidaMovBodega { get; set; }
        public int? MinutosValidaBodega { get; set; }
        public int? CCostoPayroll { get; set; } // Cambiado a nulable
        public int? TiempoRetencion { get; set; }
        public int? CantEntregas { get; set; }
        public string? CtrlBodega { get; set; }
        public string? UsuarioSupervisor { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public string? EComActivaRetiro { get; set; }
        public string? EcomActivaStock { get; set; }
        public string? EcomActivaDespacho { get; set; }
        public string? EcomActivaPedidos { get; set; }
        public string? EcomDefecto { get; set; }
        public Guid? EcomCod_Comuna { get; set; }
        public Guid? Cod_Zona { get; set; }

        [Timestamp]
        public byte[]? EcoTimeStamp { get; set; }

        public string? TbkIntegrado { get; set; }
        public string? Multicaja { get; set; }
        public string? Puerto { get; set; }
        public long? Presup_Asignado { get; set; }
        public long? Presup_Utilizado { get; set; }
        public string? RRHHValidaHuella { get; set; }
        public string? EcomForzarStock { get; set; }
        public Guid? EcomCod_Provincia { get; set; }
        public Guid? EcomCod_Region { get; set; }
        public string? EcomTiendaDespacho { get; set; }
        [Column("SucursalSII")]
        public int SucursalSii { get; set; }
        public string? DireccionSii { get; set; }
        public string? ComunaSii { get; set; }
        public string? CiudadSii { get; set; }
        public string? RegionSii { get; set; }
        public string? BoletaElec { get; set; }
        public string? LatitudNew { get; set; }
        public string? LongitudNew { get; set; }
        public int? Mt2 { get; set; }
        public int? CodigoServicioGtd { get; set; }
        public string? Horario { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string? Cod_Tienda_RFID { get; set; }
        public string? Ecommerce { get; set; }

        // Estos 3 suelen ser los que causan el error en procesos de integración
        public int? MinutosImpDocElec { get; set; }
        public int? StockLockers { get; set; }
        public int? Codigo_Talana { get; set; }

        public int? NewGrabacion { get; set; }
        public int? NewDocumentoSII { get; set; }

        // Estos campos estaban como INT simple, ahora son INT?
        public int? Multi_Activa { get; set; }
        public int? Multi_Boleta { get; set; }
        public int? Multi_Etiqueta { get; set; }

        // Campo faltante detectado en el script SQL
        public Guid? Cod_Tienda_Stock_Relacion { get; set; }
    }
}