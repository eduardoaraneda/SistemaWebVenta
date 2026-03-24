using AutoMapper;
using Domain.Entidades;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TheLine2.Infrastructure.Persistence;
using TheLine2.Models.DTOs;
// ... otros usings

public class LoginController : Controller
{
    private readonly UserManager<Usuarios> _userManager;
    private readonly SignInManager<Usuarios> _signInManager;
    private readonly ApplicationDBContext _context;
    private readonly IMapper _mapper; 

    public LoginController(
        UserManager<Usuarios> userManager,
        SignInManager<Usuarios> signInManager,
        ApplicationDBContext context,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _mapper = mapper;
    }

    // MÉTODO CON MAPPER PARA OBTENER EMPRESAS
    private async Task<IEnumerable<EmpresaDTO>> ObtenerListaEmpresas()
    {
        var entidades = await _context.Set<Empresas>().ToListAsync();
        // El Mapper convierte la lista de Entidades a una lista de DTOs
        return _mapper.Map<IEnumerable<EmpresaDTO>>(entidades);
    }

    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var empresasDto = await ObtenerListaEmpresas();

        // Ahora empresasDto tiene propiedades "Id" y "Nombre" gracias al nuevo perfil
        ViewBag.Empresas = new SelectList(empresasDto, "Id", "Nombre");
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(string username, string password, Guid EmpresaId)
    {
        var usuario = await (from u in _context.usuarios
                             join t in _context.Tiendas on u.Cod_Tienda equals t.Cod_Tienda
                             where u.Id == username && t.Cod_Empresa == EmpresaId
                             select u).FirstOrDefaultAsync();

        if (usuario != null && usuario.PasswordHash == password)
        {

            usuario.Estacion = Environment.MachineName;
            var estacion = _context.Estaciones.Where(e => e.Descripcion == usuario.Estacion).FirstOrDefault();
            var tienda = _context.Tiendas.Where(t => t.Cod_Tienda == estacion.Cod_Tienda).FirstOrDefault();

            // 1. Definir la lista de Claims correctamente
            var claims = new List<Claim>
            {
                // Claims obligatorios para que Identity funcione
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Id.ToString()),

                // Tus Claims personalizados
                new Claim("NombreCompleto", $"{usuario.Nombre} {usuario.Apellido_Pat}"),
                new Claim("EmpresaId", EmpresaId.ToString()),
                new Claim("CodTienda", estacion.Cod_Tienda.ToString()),
                new Claim("Estacion", estacion.Descripcion ?? "Desconocida"),
                new Claim("Tienda", tienda.Descripcion.ToString()),
                new Claim("RutUsuario", usuario.Id.ToString()),
                new Claim("CodEstacion", estacion.Cod_Estacion.ToString())
            };

            // 2. Crear la identidad (asegúrate de que esto esté fuera de las llaves de la lista)
            var claimsIdentity = new ClaimsIdentity(claims, "CookieAuth");


            // 3. Crear el Principal (el "dueño" de la identidad)
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = false, // Cambia a true si quieres "Recordarme"
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            };

            // 4. Iniciar sesión directamente con el esquema de Cookies de Identity
            await HttpContext.SignInAsync(
                IdentityConstants.ApplicationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Credenciales o Empresa incorrectas.");
        ViewBag.Empresas = new SelectList(await ObtenerListaEmpresas(), "Id", "Nombre");
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync(); // Esto limpia todas las cookies de Identity automáticamente
        return RedirectToAction("Index", "Login");
    }

}