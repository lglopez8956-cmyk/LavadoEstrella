using LavadoEstrella.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LavadoEstrella.Controllers;

public class ServiciosController : Controller
{
    private readonly AppDbContext _db;

    public ServiciosController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        return View(await _db.Servicios.OrderBy(s => s.Id).ToListAsync());
    }
}
