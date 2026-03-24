using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entidades
{
    [Table("Ges_Usuarios", Schema = "dbo")]
    public class Usuarios : IdentityUser<string>
    {
        [Key]
        [Column("Usuario")]
        public override string Id { get; set; } = string.Empty;

        [Required]
        [StringLength(1)]
        public string Dv_Usuario { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Apellido_Pat { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Apellido_Mat { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "char(2)")]
        public string Cod_TipoUsuario { get; set; } = string.Empty;

        public decimal Descuento_Compra { get; set; }
        public decimal Descuento_Venta { get; set; }
        public int Monto_Compra { get; set; }
        public int Periodo_Compra { get; set; }
        public int Utilizado_Compra { get; set; }

        [Required]
        [Column(TypeName = "char(1)")]
        public string Estado { get; set; } = "V";

        public Guid Cod_Tienda { get; set; }
        public int Codigo_Numerico { get; set; }

        // ... (Mantén los campos de auditoría: Usuario_Crea, Fecha_Crea, etc.)

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string? Nombre_Usuario { get; private set; }

        // MANTÉN estos porque NO están en IdentityUser
        [StringLength(1)]
        public string Tipo_Contrato { get; set; } = "I";
        [StringLength(20)]
        public string Celular { get; set; } = string.Empty;
        public Guid? Cod_Afp { get; set; }
        public Guid? Cod_Isapre { get; set; }
        public Guid? Cod_Nacionalidad { get; set; }
        public DateTime Fecha_Nacimiento { get; set; }
        public DateTime Fecha_Ingreso { get; set; }
        [StringLength(15)]
        public string Estado_Civil { get; set; } = "Sin Definir";
        [StringLength(1)]
        public string Sexo { get; set; } = string.Empty;
        public Guid Cod_Banco { get; set; }
        [StringLength(100)]
        public string CuentaBanco { get; set; } = string.Empty;
        public int HorasSemana { get; set; }

        [NotMapped]
        public bool Registrado { get; set; }
        [NotMapped]
        public string? Estacion { get; set; }
    }
}