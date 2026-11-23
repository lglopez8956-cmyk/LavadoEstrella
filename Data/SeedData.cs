using LavadoEstrella.Models;
using Microsoft.AspNetCore.Identity;

namespace LavadoEstrella.Data;

/// <summary>Datos iniciales: servicios, equipo y un usuario de demostración con historial.</summary>
public static class SeedData
{
    public const string CorreoDemo = "demo@estrella.com";
    public const string ClaveDemo = "Demo1234";

    public static void Cargar(AppDbContext db)
    {
        if (db.Servicios.Any()) return;

        var lavado = new Servicio
        {
            Nombre = "Lavado",
            Precio = 30000,
            Descripcion = "Lavado exterior completo con espuma activa, limpieza de rines y secado profesional.",
            ImagenUrl = "/img/servicio-lavado.jpg",
            Icono = "car"
        };
        var pulido = new Servicio
        {
            Nombre = "Pulido",
            Precio = 130000,
            Descripcion = "Pulido profesional que elimina imperfecciones y devuelve el brillo original.",
            ImagenUrl = "/img/servicio-pulido.jpg",
            Icono = "sparkles"
        };
        var motor = new Servicio
        {
            Nombre = "Lavado motor",
            Precio = 15000,
            Descripcion = "Limpieza segura del motor que elimina grasa y suciedad, mejorando su rendimiento.",
            ImagenUrl = "/img/servicio-motor.jpg",
            Icono = "engine"
        };
        db.Servicios.AddRange(lavado, pulido, motor);

        db.Empleados.AddRange(
            new Empleado
            {
                Nombre = "Juan Pérez",
                Cargo = "Administrador",
                Descripcion = "Líder del equipo con más de 10 años en el cuidado automotriz.",
                FotoUrl = "/img/equipo-juan.jpg"
            },
            new Empleado
            {
                Nombre = "Fernando",
                Cargo = "Especialista en Pulido",
                Descripcion = "Experto en corrección de pintura y detallado automotriz.",
                FotoUrl = "/img/equipo-fernando.jpg"
            },
            new Empleado
            {
                Nombre = "Santiago",
                Cargo = "Técnico de Motor",
                Descripcion = "Especialista en limpieza de motor y mantenimiento automotriz.",
                FotoUrl = "/img/equipo-santiago.jpg"
            });

        // Usuario de demostración para ver el perfil con historial.
        var demo = new Usuario { Nombre = "Cliente Demo", Correo = CorreoDemo, Telefono = "300 111 2233" };
        demo.ClaveHash = new PasswordHasher<Usuario>().HashPassword(demo, ClaveDemo);
        db.Usuarios.Add(demo);
        db.SaveChanges();

        var hoy = DateTime.Today;
        db.Reservas.AddRange(
            new Reserva { UsuarioId = demo.Id, ServicioId = lavado.Id, Precio = lavado.Precio, Estado = EstadoReserva.Completada, FechaHora = hoy.AddDays(-45).AddHours(9) },
            new Reserva { UsuarioId = demo.Id, ServicioId = pulido.Id, Precio = pulido.Precio, Estado = EstadoReserva.Completada, FechaHora = hoy.AddDays(-30).AddHours(14) },
            new Reserva { UsuarioId = demo.Id, ServicioId = motor.Id, Precio = motor.Precio, Estado = EstadoReserva.Completada, FechaHora = hoy.AddDays(-12).AddHours(10) },
            new Reserva { UsuarioId = demo.Id, ServicioId = lavado.Id, Precio = lavado.Precio, Estado = EstadoReserva.Cancelada, FechaHora = hoy.AddDays(-5).AddHours(11) },
            new Reserva { UsuarioId = demo.Id, ServicioId = lavado.Id, Precio = lavado.Precio, Estado = EstadoReserva.Confirmada, FechaHora = hoy.AddDays(3).AddHours(10) });
        db.SaveChanges();
    }
}
