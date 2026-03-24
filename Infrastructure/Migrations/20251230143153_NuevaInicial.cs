using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class NuevaInicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "Ges_Roles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Ges_Roles", x => x.Id); });


            migrationBuilder.CreateTable(
                name: "Ges_RolPermisos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ges_RolPermisos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ges_RolPermisos_Ges_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Ges_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // --- TABLAS CON REFERENCIA A USUARIOS (CORREGIDAS A varchar(20)) ---

            migrationBuilder.CreateTable(
                name: "Ges_UsuarioLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "varchar(20)", nullable: false) // CORREGIDO
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ges_UsuarioLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_Ges_UsuarioLogins_Ges_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Ges_Usuarios",
                        principalColumn: "Usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ges_UsuarioPermisos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "varchar(20)", nullable: false), // CORREGIDO
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ges_UsuarioPermisos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ges_UsuarioPermisos_Ges_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Ges_Usuarios",
                        principalColumn: "Usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ges_UsuarioRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(20)", nullable: false), // CORREGIDO
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ges_UsuarioRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_Ges_UsuarioRoles_Ges_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Ges_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ges_UsuarioRoles_Ges_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Ges_Usuarios",
                        principalColumn: "Usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ges_UsuarioTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(20)", nullable: false), // CORREGIDO
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ges_UsuarioTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_Ges_UsuarioTokens_Ges_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Ges_Usuarios",
                        principalColumn: "Usuario",
                        onDelete: ReferentialAction.Cascade);
                });

            // --- INDICES ---
            migrationBuilder.CreateIndex(name: "RoleNameIndex", table: "Ges_Roles", column: "NormalizedName", unique: true, filter: "[NormalizedName] IS NOT NULL");
            migrationBuilder.CreateIndex(name: "IX_Ges_RolPermisos_RoleId", table: "Ges_RolPermisos", column: "RoleId");
            migrationBuilder.CreateIndex(name: "IX_Ges_UsuarioLogins_UserId", table: "Ges_UsuarioLogins", column: "UserId");
            migrationBuilder.CreateIndex(name: "IX_Ges_UsuarioPermisos_UserId", table: "Ges_UsuarioPermisos", column: "UserId");
            migrationBuilder.CreateIndex(name: "IX_Ges_UsuarioRoles_RoleId", table: "Ges_UsuarioRoles", column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Ges_RolPermisos");
            migrationBuilder.DropTable(name: "Ges_SubMenus");
            migrationBuilder.DropTable(name: "Ges_UsuarioLogins");
            migrationBuilder.DropTable(name: "Ges_UsuarioPermisos");
            migrationBuilder.DropTable(name: "Ges_UsuarioRoles");
            migrationBuilder.DropTable(name: "Ges_UsuarioTokens");
            migrationBuilder.DropTable(name: "Ges_Menus");
            migrationBuilder.DropTable(name: "Ges_Roles");
        }
    }
}