# Sistema Web de Ventas

Sistema de gestión comercial con punto de venta, consulta de documentos y módulos de tiendas, estaciones y bodegas. Organizado en capas para separar presentación, aplicación, dominio e infraestructura.

## Tecnologías

C# · .NET 10 · ASP.NET Core MVC · Razor · SQL Server · Entity Framework Core · Dapper · ASP.NET Core Identity · AutoMapper · TypeScript · jQuery.

## Arquitectura

| Proyecto | Responsabilidad |
| --- | --- |
| `Domain/` | Entidades del negocio. |
| `Application/` | Contratos y modelos de aplicación. |
| `Infrastructure/` | Persistencia y repositorios. |
| `TheLine2/` | Aplicación web, controladores, vistas y recursos de interfaz. |

## Preparar y compilar

Instala el SDK de .NET 10 y SQL Server. Desde la raíz:

```powershell
dotnet restore TheLine2.slnx
dotnet build TheLine2.slnx
$env:ConnectionStrings__DefaultConnection = "Server=localhost;Database=SistemaWebVenta;Integrated Security=True;Encrypt=True;TrustServerCertificate=True"
dotnet run --project TheLine2/Web.csproj
```

La conexión es un ejemplo para desarrollo local con autenticación de Windows. Abre la URL indicada por la terminal; el acceso está en `/Login/Index`.

**La conexión por sí sola no prepara la base.** Los repositorios utilizan tablas y procedimientos almacenados de un entorno comercial. No se incluye un paquete SQL completo para reconstruirlo desde cero. También necesitas usuarios y roles compatibles con su modelo Identity.

`TheLine2/package.json` declara TypeScript y tipos de jQuery; no incluye una tarea npm de compilación ni pruebas funcionales. Revisa la configuración del proyecto antes de modificar el frontend.

## Proyecto relacionado

[Comercial Manufacturera](https://github.com/eduardoaraneda/comercial-manufacturera) es otro proyecto del portafolio, con scripts de base de datos, pruebas y [demo en línea](https://portafolio.somee.com).

## Configuración y alcance

Usa una base de desarrollo y credenciales propias. Configura secretos mediante variables de entorno o User Secrets; no los incluyas en commits. La compilación no comprueba la disponibilidad de bases de datos, SMTP o APIs externas.

## Autor

[Eduardo Araneda](https://github.com/eduardoaraneda) · [Portafolio](https://eduardoaraneda.github.io/Portafolio/)
