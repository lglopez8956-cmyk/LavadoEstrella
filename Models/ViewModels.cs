using System.ComponentModel.DataAnnotations;

namespace LavadoEstrella.Models;

public class LoginVM
{
    [Display(Name = "Correo electrónico")]
    [Required(ErrorMessage = "Escribe tu correo."), EmailAddress(ErrorMessage = "El correo no es válido.")]
    public string Correo { get; set; } = "";

    [Display(Name = "Contraseña")]
    [Required(ErrorMessage = "Escribe tu contraseña.")]
    [DataType(DataType.Password)]
    public string Clave { get; set; } = "";

    public string? ReturnUrl { get; set; }
}

public class RegistroVM
{
    [Display(Name = "Nombre completo")]
    [Required(ErrorMessage = "Escribe tu nombre."), StringLength(80)]
    public string Nombre { get; set; } = "";

    [Display(Name = "Correo electrónico")]
    [Required(ErrorMessage = "Escribe tu correo."), EmailAddress(ErrorMessage = "El correo no es válido.")]
    [StringLength(120)]
    public string Correo { get; set; } = "";

    [Display(Name = "Teléfono")]
    [Required(ErrorMessage = "Escribe tu teléfono."), StringLength(30)]
    public string Telefono { get; set; } = "";

    [Display(Name = "Contraseña")]
    [Required(ErrorMessage = "Crea una contraseña.")]
    [StringLength(60, MinimumLength = 6, ErrorMessage = "Mínimo 6 caracteres.")]
    [DataType(DataType.Password)]
    public string Clave { get; set; } = "";

    [Display(Name = "Confirmar contraseña")]
    [Required(ErrorMessage = "Repite la contraseña.")]
    [Compare(nameof(Clave), ErrorMessage = "Las contraseñas no coinciden.")]
    [DataType(DataType.Password)]
    public string ConfirmarClave { get; set; } = "";
}

public class ReservaVM
{
    [Display(Name = "Servicio")]
    [Required(ErrorMessage = "Elige un servicio.")]
    public int? ServicioId { get; set; }

    [Display(Name = "Fecha")]
    [Required(ErrorMessage = "Elige una fecha.")]
    public DateTime? Fecha { get; set; }

    [Display(Name = "Hora")]
    [Required(ErrorMessage = "Elige una hora.")]
    public int? Hora { get; set; }
}

public class ActualizarPerfilVM
{
    [Required(ErrorMessage = "Escribe tu nombre."), StringLength(80)]
    public string Nombre { get; set; } = "";

    [Required(ErrorMessage = "Escribe tu teléfono."), StringLength(30)]
    public string Telefono { get; set; } = "";
}

public class PerfilVM
{
    public Usuario Usuario { get; set; } = new();
    public List<Reserva> Proximas { get; set; } = new();
    public List<Reserva> Historial { get; set; } = new();
    public decimal TotalInvertido { get; set; }
    public int ServiciosRealizados { get; set; }
}
