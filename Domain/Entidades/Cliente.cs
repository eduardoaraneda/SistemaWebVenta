using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entidades
{
    [Table("Ges_Clientes", Schema = "dbo")]
    public class Clientes
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Cod_Cliente { get; set; }

        public int Rut_Cliente { get; set; }

        [Required]
        [Column(TypeName = "char(1)")]
        public string Dv_Cliente { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Nombres { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string ApellidoPat { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string ApellidoMat { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Giro { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Direccion { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Numero { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Departamento { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Poblacion { get; set; } = string.Empty;

        public Guid Cod_Region { get; set; }

        public Guid Cod_Ciudad { get; set; }

        public Guid Cod_Comuna { get; set; }

        [Required]
        [StringLength(15)]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        public string Celular { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Mail { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        public string Fax { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "char(1)")]
        public string Tipo_Cliente { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "text")]
        public string Observaciones { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "char(1)")]
        public string Estado { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string EstadoCivil { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "char(1)")]
        public string Sexo { get; set; } = "M";

        public DateTime FechaNacimiento { get; set; }

        [Required]
        [StringLength(150)]
        public string Facebook { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "char(1)")]
        public string Publicidad { get; set; } = "N";

        public Guid Cod_Profesion { get; set; }

        // Columnas Calculadas en SQL
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string? Cliente { get; private set; }

        [Required]
        [StringLength(1)]
        public string Convenio { get; set; } = "N";

        public DateTime FechaConvenioDesde { get; set; }

        public DateTime FechaConvenioHasta { get; set; }

        public Guid? Cod_ClienteConvenio { get; set; }

        [Required]
        [StringLength(20)]
        public string Usuario_Creacion { get; set; } = string.Empty;

        public DateTime Fecha_Creacion { get; set; }

        [Required]
        [StringLength(20)]
        public string Usuario_Modificacion { get; set; } = string.Empty;

        public DateTime Fecha_Modificacion { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string? Direccion_Completa { get; private set; }

        [Required]
        [StringLength(20)]
        public string OrigenUsuario { get; set; } = "ERP";

        [StringLength(20)]
        public string? EcomUsuario { get; set; }

        [StringLength(50)]
        public string? EcomPassword { get; set; }

        [StringLength(150)]
        public string? EcomMail { get; set; }

        public Guid? EcomCod_Tienda { get; set; }

        [StringLength(100)]
        public string? Banco { get; set; }

        public Guid? TIpoCuenta { get; set; }

        [StringLength(50)]
        public string? NroCuenta { get; set; }

        public int? NroTicket { get; set; }

        [StringLength(10)]
        public string? RutCuenta { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string? Rut_Completo { get; private set; }

        [Required]
        [StringLength(100)]
        public string Id_Dynamics { get; set; } = string.Empty;

        public Guid? Id_Clientes_Dyn { get; set; }
        [NotMapped]
        public bool? Registrado { get; set; }
    }
}