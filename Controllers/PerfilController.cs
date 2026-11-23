using LavadoEstrella.Data;
using LavadoEstrella.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LavadoEstrella.Controllers;

[Authorize]
public class PerfilController : Controller
{
    private readonly AppDbContext _db;

    public PerfilController(AppDbContext db) => _db = db;

    // Mi perfil: datos de la persona, próximas reservas e historial de servicios.
    public async Task<IActionResult> Index()
    {
        var usuarioId = User.IdUsuario()!.Value;
        var usuario = await _db.Usuarios.FindAsync(usuarioId);
        if (usuario is null)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Cuenta");
        }

        var ahora = DateTime.Now;

        // Las reservas confirmadas cuya fecha ya pasó se consideran servicios realizados.
        var vencidas = await _db.Reservas
            .Where(r => r.UsuarioId == usuarioId && r.Estado == EstadoReserva.Confirmada && r.FechaHora < ahora)
            .ToListAsync();
        if (vencidas.Count > 0)
        {
            foreach (var r in vencidas) r.Estado = EstadoReserva.Completada;
            await _db.SaveChangesAsync();
        }

        var reservas = await _db.Reservas
            .Include(r => r.Servicio)
            .Where(r => r.UsuarioId == usuarioId)
            .ToListAsync();

        var vm = new PerfilVM
        {
            Usuario = usuario,
            Proximas = reservas
                .Where(r => r.Estado == EstadoReserva.Confirmada)
                .OrderBy(r => r.FechaHora)
                .ToList(),
            Historial = reservas
                .Where(r => r.Estado != EstadoReserva.Confirmada)
                .OrderByDescending(r => r.FechaHora)
                .ToList(),
            TotalInvertido = reservas.Where(r => r.Estado == EstadoReserva.Completada).Sum(r => r.Precio),
            ServiciosRealizados = reservas.Count(r => r.Estado == EstadoReserva.Completada)
        };
        return View(vm);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Actualizar(ActualizarPerfilVM modelo)
    {
        if (!ModelState.IsValid)
        {
            TempData["Mensaje"] = "Revisa tu nombre y teléfono: no pueden quedar vacíos.";
            return RedirectToAction(nameof(Index));
        }

        var usuario = await _db.Usuarios.FindAsync(User.IdUsuario()!.Value);
        if (usuario is null) return RedirectToAction("Login", "Cuenta");

        usuario.Nombre = modelo.Nombre.Trim();
        usuario.Telefono = modelo.Telefono.Trim();
        await _db.SaveChangesAsync();

        TempData["Mensaje"] = "Tus datos se actualizaron. El nombre nuevo se verá la próxima vez que ingreses.";
        return RedirectToAction(nameof(Index));
    }
}
