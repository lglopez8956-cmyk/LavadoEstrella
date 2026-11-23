using LavadoEstrella.Data;
using LavadoEstrella.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LavadoEstrella.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;

    public HomeController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        ViewData["Servicios"] = await _db.Servicios.OrderBy(s => s.Id).ToListAsync();
        ViewData["Empleados"] = await _db.Empleados.OrderBy(e => e.Id).ToListAsync();
        return View();
    }

    public async Task<IActionResult> Nosotros()
    {
        return View(await _db.Empleados.OrderBy(e => e.Id).ToListAsync());
    }

    public IActionResult Promociones() => View();

    public IActionResult Contacto() => View();
}
