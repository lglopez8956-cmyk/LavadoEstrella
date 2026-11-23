using System.ComponentModel.DataAnnotations;

namespace LavadoEstrella.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    public string Nombre { get; set; } = "";

    [Required, StringLength(120)]
    public string Correo { get; set; } = "";

    [StringLength(30)]
    public string Telefono { get; set; } = "";

    public string ClaveHash { get; set; } = "";

    public DateTime CreadoEn { get; set; } = DateTime.Now;

    public List<Reserva> Reservas { get; set; } = new();
}

public class Servicio
{
    public int Id { get; set; }

    [Required, StringLength(60)]
    public string Nombre { get; set; } = "";

    [StringLength(300)]
    public string Descripcion { get; set; } = "";

    public decimal Precio { get; set; }

    /// <summary>Foto del servicio (ruta dentro de wwwroot).</summary>
    public string ImagenUrl { get; set; } = "";

    /// <summary>Nombre del ícono (ver Iconos.cs): car, sparkles, engine…</summary>
    public string Icono { get; set; } = "car";
}

public class Empleado
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    public string Nombre { get; set; } = "";

    [StringLength(60)]
    public string Cargo { get; set; } = "";

    [StringLength(200)]
    public string Descripcion { get; set; } = "";

    public string FotoUrl { get; set; } = "";
}

public enum EstadoReserva
{
    Confirmada,
    Completada,
    Cancelada
}

/// <summary>Un servicio reservado/comprado por un usuario. Forma el historial del perfil.</summary>
public class Reserva
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public int ServicioId { get; set; }
    public Servicio? Servicio { get; set; }

    public DateTime FechaHora { get; set; }

    public EstadoReserva Estado { get; set; } = EstadoReserva.Confirmada;

    /// <summary>Precio al momento de reservar (si el precio cambia después, el historial no cambia).</summary>
    public decimal Precio { get; set; }

    public DateTime CreadaEn { get; set; } = DateTime.Now;
}

/// <summary>Horario de atención y cupo por hora.</summary>
public static class Agenda
{
    public const int Apertura = 8;
    public const int UltimaHora = 17;
    public const int CupoPorHora = 2;

    public static IEnumerable<int> Horas => Enumerable.Range(Apertura, UltimaHora - Apertura + 1);
}

public class NegocioOpciones
{
    public string Nombre { get; set; } = "Estrella";
    public string Lema { get; set; } = "Lavado especializado";
    public string Telefono { get; set; } = "";
    public string WhatsApp { get; set; } = "";
    public string Facebook { get; set; } = "";
    public string Direccion { get; set; } = "";
    public string Horario { get; set; } = "";
}
