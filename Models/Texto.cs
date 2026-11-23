using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Html;

namespace LavadoEstrella.Models;

/// <summary>Textos y formatos para las vistas.</summary>
public static class Texto
{
    /// <summary>30000 → $30.000</summary>
    public static string Pesos(decimal valor) =>
        "$" + valor.ToString("N0", CultureInfo.InvariantCulture).Replace(",", ".");

    public static string Nombre(this EstadoReserva estado) => estado switch
    {
        EstadoReserva.Confirmada => "Confirmada",
        EstadoReserva.Completada => "Realizado",
        EstadoReserva.Cancelada => "Cancelada",
        _ => estado.ToString()
    };

    public static string Fecha(DateTime fecha) => fecha.ToString("dd/MM/yyyy");

    public static string Hora(int hora) => new DateTime(2000, 1, 1, hora, 0, 0).ToString("hh:mm tt", CultureInfo.InvariantCulture);

    public static string Iniciales(string nombre)
    {
        var partes = nombre.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (partes.Length == 0) return "?";
        if (partes.Length == 1) return partes[0][..1].ToUpperInvariant();
        return (partes[0][..1] + partes[1][..1]).ToUpperInvariant();
    }

    public static int? IdUsuario(this ClaimsPrincipal usuario)
    {
        var valor = usuario.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(valor, out var id) ? id : null;
    }
}

/// <summary>Íconos SVG sencillos (trazo), pensados para usarse como @Iconos.Svg("calendar").</summary>
public static class Iconos
{
    public static HtmlString Svg(string nombre, int tamano = 24)
    {
        var contenido = nombre switch
        {
            "calendar" => "<rect x=\"3\" y=\"4\" width=\"18\" height=\"17\" rx=\"2\"/><path d=\"M8 2v4M16 2v4M3 10h18\"/><rect x=\"8\" y=\"13\" width=\"5\" height=\"5\" rx=\"1\"/>",
            "clock" => "<circle cx=\"12\" cy=\"12\" r=\"9\"/><path d=\"M12 7v5l3 2\"/>",
            "shield" => "<path d=\"M12 3l8 3v6c0 5-3.5 8-8 9-4.5-1-8-4-8-9V6z\"/><path d=\"M8.5 12l2.5 2.5L16 9.5\"/>",
            "sparkles" => "<path d=\"M12 3l1.8 4.7L18.5 9.5l-4.7 1.8L12 16l-1.8-4.7L5.5 9.5l4.7-1.8z\"/><path d=\"M19 15l.8 2.2L22 18l-2.2.8L19 21l-.8-2.2L16 18l2.2-.8z\"/>",
            "car" => "<path d=\"M5 16l1.5-5.5A2 2 0 0 1 8.4 9h7.2a2 2 0 0 1 1.9 1.5L19 16\"/><rect x=\"3\" y=\"16\" width=\"18\" height=\"4\" rx=\"1.5\"/><path d=\"M7 18h.01M17 18h.01\"/>",
            "engine" => "<path d=\"M4 10h2l1-2h6l1 2h2v7H4z\"/><path d=\"M20 12v3M2 12v3M9 8V5h4\"/>",
            "star" => "<path d=\"M12 3l2.9 6 6.6.9-4.8 4.6 1.2 6.5L12 17.4 6.1 21l1.2-6.5L2.5 9.9l6.6-.9z\"/>",
            "leaf" => "<path d=\"M5 19C5 10 11 4 20 4c0 9-6 15-15 15z\"/><path d=\"M5 19l8-8\"/>",
            "user" => "<circle cx=\"12\" cy=\"8\" r=\"4\"/><path d=\"M4 21c1-4 4-6 8-6s7 2 8 6\"/>",
            "check" => "<path d=\"M5 12.5l4.5 4.5L19 7.5\"/>",
            "whatsapp" => "<path d=\"M4 20l1.3-4A8 8 0 1 1 8.2 18.8z\"/><path d=\"M9 9c0 3 3 6 6 6l1-1.5-2-1-1 .8c-1-.4-2-1.4-2.4-2.4l.8-1-1-2z\"/>",
            "facebook" => "<path d=\"M14 8h2V4h-2.5A3.5 3.5 0 0 0 10 7.5V10H8v4h2v7h4v-7h2.2l.6-4H14V7.8c0-.5.3-.8 1-.8z\"/>",
            "pin" => "<path d=\"M12 21s7-6 7-11a7 7 0 1 0-14 0c0 5 7 11 7 11z\"/><circle cx=\"12\" cy=\"10\" r=\"2.5\"/>",
            "phone" => "<path d=\"M5 4h4l2 5-2.5 1.5a11 11 0 0 0 5 5L15 13l5 2v4a2 2 0 0 1-2 2A16 16 0 0 1 3 6a2 2 0 0 1 2-2z\"/>",
            _ => "<circle cx=\"12\" cy=\"12\" r=\"9\"/>"
        };

        return new HtmlString(
            $"<svg class=\"icono\" width=\"{tamano}\" height=\"{tamano}\" viewBox=\"0 0 24 24\" fill=\"none\" " +
            $"stroke=\"currentColor\" stroke-width=\"1.8\" stroke-linecap=\"round\" stroke-linejoin=\"round\" aria-hidden=\"true\">{contenido}</svg>");
    }
}
