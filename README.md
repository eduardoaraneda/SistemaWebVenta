# TheLine2

Proyecto de ventas Point-of-Sale (Punto de Venta) basado en .NET 10 y Razor Pages / ASP.NET Core.

## Resumen
Aplicación interna para gestión de ventas, boletas/facturas, notas de crédito y consultas de documentos. Contiene controladores MVC y vistas Razor.

## Requisitos
- .NET 10 SDK
- Visual Studio 2022/2026 o VS Code
- SQL Server accesible para la cadena de conexión (DefaultConnection)
- Git

## Configuración
1. Copia `appsettings.example.json` a `appsettings.json` (si existe) o agrega tu `appsettings.json` local.
2. Configure la cadena de conexión en `ConnectionStrings:DefaultConnection` para apuntar a tu base de datos.
3. No subas archivos con credenciales o claves al repositorio. El archivo `.gitignore` ya incluye reglas para omitir `appsettings.*.json`, `.env`, `secrets.json`, certificados y claves privadas.

Si usas `dotnet user-secrets`:

```powershell
cd "C:\ruta\a\TheLine2"
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=...;Database=...;User Id=...;Password=...;"
```

## Ejecutar localmente
1. Abrir la solución en Visual Studio o desde terminal:

```powershell
cd "C:\Users\earaneda\OneDrive - International Sport S.A\Documentos\Migracion\TheLine2\TheLine2"
dotnet build
dotnet run --project TheLine2
```

2. Abre el navegador en `https://localhost:5001` o la URL indicada por la ejecución.

## Scripts útiles
- `dotnet build` — Compila la solución
- `dotnet run --project TheLine2` — Ejecuta la aplicación

## Base de datos
La aplicación depende de procedimientos almacenados y tablas que deben existir en la base de datos. Revisa la carpeta de migraciones o el script SQL proporcionado por el equipo para crear la estructura necesaria.

## Contribuir
- Crea una branch a partir de `master`:

```powershell
git checkout -b feature/nombre-feature
```

- Haz commits pequeños y descriptivos.
- Abre pull request hacia `master`.

## Subir al remoto (GitHub Desktop)
Sigue los pasos en GitHub Desktop: Add local repository → selecciona la carpeta del proyecto → Commit to master → Push origin o Publish repository.

O por terminal:

```powershell
git add .
git commit -m "Initial commit"
git push origin master
```

## Notas de seguridad
- Nunca incluyas archivos con contraseñas, certificados privados o tokens en el repositorio.
- Revisa `.gitignore` antes de hacer commit.

---

Si necesitas que añada instrucciones específicas sobre la base de datos, los endpoints más importantes o cómo ejecutar pruebas, dime cuáles y los añado al README.