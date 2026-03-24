using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheLine2.Infrastructure.Persistence;

public class MenuViewComponent : ViewComponent
{
    private readonly ApplicationDBContext _context;

    public MenuViewComponent(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // Traemos los menús con sus submenús incluidos
        var items = await _context.Ges_Menus
                                  .Include(m => m.SubMenus)
                                  .ToListAsync();
        return View(items);
    }
}