using LavadoEstrella.Data;
using LavadoEstrella.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LavadoEstrella.Controllers;

[Authorize]
public class ReservasController : Controller
{
    private readonly AppDbContext _db;

    public ReservasController(AppDbContext db) => _db = db;

    // Se llega desde "Reservar", desde el formulario del inicio o desde un servicio.
    // Si la persona no ha iniciado sesión, el sistema la lleva a Ingresar y luego regresa aquí.
    [HttpGet]
    public async Task<IActionResult> Crear(int? servicioId, DateTime? fecha, int? hora)
    {
        await CargarServicios();
        return View(new ReservaVM
        {
            ServicioId = servicioId,
            Fecha = fecha ?? DateTime.Today.AddDays(1),
            Hora = hora
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(ReservaVM modelo)
    {
        Servicio? servicio = null;
        if (modelo.ServicioId.HasValue)
        {
            servicio = await _db.Servicios.FindAsync(modelo.ServicioId.Value);
            if (servicio is null) ModelState.AddModelError(nameof(modelo.ServicioId), "Ese servicio no existe.");
        }

        DateTime? fechaHora = null;
        if (modelo.Fecha.HasValue && modelo.Hora.HasValue)
        {
            var hora = modelo.Hora.Value;
            if (hora < Agenda.Apertura || hora > Agenda.UltimaHora)
            {
                ModelState.AddModelError(nameof(modelo.Hora), "Elige una hora dentro del horario de atención.");
            }
            else
            {
                var candidata = modelo.Fecha.Value.Date.AddHours(hora);
                if (candidata <= DateTime.Now)
                {
                    ModelState.AddModelError(nameof(modelo.Fecha), "Elige una fecha y hora que todavía no hayan pasado.");
                }
                else
                {
                    var ocupadas = await _db.Reservas.CountAsync(r =>
                        r.FechaHora == candidata && r.Estado == EstadoReserva.Confirmada);
                    if (ocupadas >= Agenda.CupoPorHora)
                        ModelState.AddModelError(nameof(modelo.Hora), "Esa hora ya está llena. Elige otra.");
                    else
                        fechaHora = candidata;
                }
            }
        }

        if (!ModelState.IsValid || servicio is null || fechaHora is null)
        {
            await CargarServicios();
            return View(modelo);
        }

        _db.Reservas.Add(new Reserva
        {
            UsuarioId = User.IdUsuario()!.Value,
            ServicioId = servicio.Id,
            FechaHora = fechaHora.Value,
            Precio = servicio.Precio,
            Estado = EstadoReserva.Confirmada
        });
        await _db.SaveChangesAsync();

        TempData["Mensaje"] = $"Reserva confirmada: {servicio.Nombre}, {Texto.Fecha(fechaHora.Value)} a las {Texto.Hora(fechaHora.Value.Hour)}.";
        return RedirectToAction("Index", "Perfil");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancelar(int id)
    {
        var usuarioId = User.IdUsuario()!.Value;
        var reserva = await _db.Reservas.FirstOrDefaultAsync(r => r.Id == id && r.UsuarioId == usuarioId);
        if (reserva is null) return NotFound();

        if (reserva.Estado == EstadoReserva.Confirmada && reserva.FechaHora > DateTime.Now)
        {
            reserva.Estado = EstadoReserva.Cancelada;
            await _db.SaveChangesAsync();
            TempData["Mensaje"] = "La reserva fue cancelada.";
        }
        return RedirectToAction("Index", "Perfil");
    }

    private async Task CargarServicios()
    {
        ViewData["Servicios"] = await _db.Servicios.OrderBy(s => s.Id).ToListAsync();
    }
}
