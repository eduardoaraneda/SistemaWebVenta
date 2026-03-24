using Domain.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TuProyecto.Models;

namespace TheLine2.Infrastructure.Persistence
{
    public class ApplicationDBContext : IdentityDbContext<Usuarios, IdentityRole<string>, string>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }
        public DbSet<Estacion> Estaciones { get; set; }
        public DbSet<Empresas> Empresas { get; set; }
        public DbSet<Tienda> Tiendas { get; set; }
        public DbSet<Menu> Ges_Menus { get; set; }
        public DbSet<SubMenu> Ges_SubMenus { get; set; }
        public DbSet<Bodegas> Ges_Bodegas { get; set; }
        public DbSet<Usuarios> usuarios { get; set; }
        public DbSet<Parametros> parametros { get; set; }
        public DbSet<Comuna> comuna { get; set; }
        public DbSet<Ciudad> ciudad { get; set; }
        public DbSet<Clientes> clientes { get; set; }
        public DbSet<Productos> Productos { get; set; }
        public DbSet<EstiloColor> estiloColors { get; set; }
        public DbSet<StockProductos> StockProductos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ResultadoVentaSii>().HasNoKey();
            base.OnModelCreating(builder);

            builder.Entity<Usuarios>(entity =>
            {
                entity.ToTable("Ges_Usuarios");

                entity.Property(e => e.Id).HasColumnName("Usuario");
                entity.Ignore(u => u.UserName); 

                entity.Property(e => e.Email).HasColumnName("Mail");
                entity.Property(e => e.PhoneNumber).HasColumnName("Telefono");
                entity.Property(e => e.PasswordHash).HasColumnName("Contraseña");

                entity.Property(e => e.EmailConfirmed).HasColumnName("EmailConfirmed");
                entity.Property(e => e.NormalizedEmail).HasColumnName("NormalizedEmail");
                entity.Property(e => e.NormalizedUserName).HasColumnName("NormalizedUserName");
                entity.Property(e => e.SecurityStamp).HasColumnName("SecurityStamp");
                entity.Property(e => e.ConcurrencyStamp).HasColumnName("ConcurrencyStamp");
                entity.Property(e => e.TwoFactorEnabled).HasColumnName("TwoFactorEnabled");
                entity.Property(e => e.LockoutEnd).HasColumnName("LockoutEnd");
                entity.Property(e => e.LockoutEnabled).HasColumnName("LockoutEnabled");
                entity.Property(e => e.AccessFailedCount).HasColumnName("AccessFailedCount");

                entity.Ignore(u => u.PhoneNumberConfirmed);
            });
        

        builder.Entity<Empresas>(entity =>
            {
                entity.ToTable("Ges_empresas");
                entity.HasKey(e => e.Cod_Empresa); 
            });

            builder.Entity<Tienda>(entity =>
            {
                entity.ToTable("Ges_Tiendas");
                entity.HasKey(t => t.Cod_Tienda); 
                entity.Property(t => t.Cod_Empresa); 
            });

            builder.Entity<IdentityRole<string>>().ToTable("Ges_Roles");

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("Ges_UsuarioRoles");
                entity.Property(e => e.UserId).HasColumnType("varchar(20)");
            });

            builder.Entity<IdentityUserClaim<string>>().ToTable("Ges_UsuarioPermisos");
            builder.Entity<IdentityUserLogin<string>>().ToTable("Ges_UsuarioLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("Ges_RolPermisos");
            builder.Entity<IdentityUserToken<string>>().ToTable("Ges_UsuarioTokens");

            builder.Entity<Menu>().ToTable("Ges_Menus");
            builder.Entity<SubMenu>().ToTable("Ges_SubMenus");

            builder.Entity<Menu>(entity => {
                entity.ToTable("Ges_Menus"); 
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Descripcion).HasColumnName("Descripcion");
                entity.Property(m => m.IconoMenu).HasColumnName("IconoMenu");
            });

            builder.Entity<SubMenu>(entity => {
                entity.ToTable("Ges_SubMenus"); 
                entity.HasKey(s => s.Id);
                // Configurar la relación
                entity.HasOne(s => s.Menu)
                      .WithMany(m => m.SubMenus)
                      .HasForeignKey("MenuId"); 
            });

            builder.Entity<Bodegas>(entity =>
            {
                entity.ToTable("Ges_Bodegas", "dbo");

                // Definimos la PK real
                entity.HasKey(e => e.Cod_Bodega);

                // Mapeos de columna para evitar que busque "Id"
                entity.Property(e => e.Cod_Bodega).HasColumnName("Cod_Bodega");
                entity.Property(e => e.Cod_Tienda).HasColumnName("Cod_Tienda");
                entity.Property(e => e.Descripcion).HasColumnName("Descripcion");
                entity.Property(e => e.Estado).HasColumnName("Estado");

               

            });

        }
    }
}