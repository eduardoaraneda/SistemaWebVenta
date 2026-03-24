using Domain.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheLine2.Infrastructure.Persistence;
using TheLine2.Models;

public class MenuController : Controller
{
    private readonly ApplicationDBContext _context;

    public MenuController(ApplicationDBContext context)
    {
        _context = context;
    }

    // Listado jerárquico
    public async Task<IActionResult> Index()
    {
        var menus = await _context.Ges_Menus.ToListAsync();
        var submenus = await _context.Ges_SubMenus.ToListAsync();
        var listaCompleta = new List<MenuListaViewModel>();

        foreach (var m in menus)
        {
            listaCompleta.Add(new MenuListaViewModel
            {
                Id = m.Id,
                Descripcion = m.Descripcion,
                Icono = m.IconoMenu,
                EsSubmenu = false
            });

            var hijos = submenus.Where(s => s.MenuId == m.Id);
            foreach (var s in hijos)
            {
                listaCompleta.Add(new MenuListaViewModel
                {
                    Id = s.Id,
                    Descripcion = s.Descripcion,
                    Icono = s.IconoSubMenu,
                    Controller = s.Controller,
                    Action = s.Action,
                    EsSubmenu = true
                });
            }
        }
        return View(listaCompleta);
    }

    [HttpPost]
    public async Task<IActionResult> SaveMenu(MenuListaViewModel model)
    {
        try
        {
            if (!model.EsSubmenu)
            {
                var nuevoMenu = new Menu
                {
                    Id = Guid.NewGuid(),
                    Descripcion = model.Descripcion,
                    IconoMenu = model.Icono
                };
                _context.Ges_Menus.Add(nuevoMenu);
            }
            else
            {
                // 1. Verificamos que el valor no sea nulo antes de intentar el Parse
                if (string.IsNullOrEmpty(model.SelectedMenuId?.ToString()) && string.IsNullOrEmpty(model.NombrePadre))
                {
                    return Json(new { success = false, message = "El ID del menú padre es requerido para crear un submenú." });
                }

                // 2. Intentamos obtener el ID del campo que esté llegando (ajusta según tu ViewModel)
                string idPadreRaw = model.SelectedMenuId?.ToString() ?? model.NombrePadre;

                var nuevoSub = new SubMenu
                {
                    Id = Guid.NewGuid(),
                    Descripcion = model.Descripcion,
                    IconoSubMenu = model.Icono,
                    // Usamos TryParse o verificamos antes para evitar el error 'input'
                    MenuId = Guid.Parse(idPadreRaw),
                    Controller = model.Controller,
                    Action = model.Action
                };

                _context.Ges_SubMenus.Add(nuevoSub);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Error: " + ex.Message });
        }
    }
}