using System.Security.Claims;
using LavadoEstrella.Data;
using LavadoEstrella.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LavadoEstrella.Controllers;

public class CuentaController : Controller
{
    private readonly AppDbContext _db;
    private readonly PasswordHasher<Usuario> _hasher = new();

    public CuentaController(AppDbContext db) => _db = db;

    [HttpGet]
    public IActionResult Login(string? returnUrl)
    {
        return View(new LoginVM { ReturnUrl = returnUrl });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVM modelo)
    {
        if (!ModelState.IsValid) return View(modelo);

        var correo = modelo.Correo.Trim().ToLowerInvariant();
        var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);

        if (usuario is null ||
            _hasher.VerifyHashedPassword(usuario, usuario.ClaveHash, modelo.Clave) == PasswordVerificationResult.Failed)
        {
            ModelState.AddModelError("", "Correo o contraseña incorrectos.");
            return View(modelo);
        }

        await Entrar(usuario);
        return Redirigir(modelo.ReturnUrl);
    }

    [HttpGet]
    public IActionResult Registro() => View(new RegistroVM());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Registro(RegistroVM modelo)
    {
        if (!ModelState.IsValid) return View(modelo);

        var correo = modelo.Correo.Trim().ToLowerInvariant();
        if (await _db.Usuarios.AnyAsync(u => u.Correo == correo))
        {
            ModelState.AddModelError(nameof(modelo.Correo), "Ya existe una cuenta con ese correo.");
            return View(modelo);
        }

        var usuario = new Usuario
        {
            Nombre = modelo.Nombre.Trim(),
            Correo = correo,
            Telefono = modelo.Telefono.Trim()
        };
        usuario.ClaveHash = _hasher.HashPassword(usuario, modelo.Clave);
        _db.Usuarios.Add(usuario);
        await _db.SaveChangesAsync();

        await Entrar(usuario);
        TempData["Mensaje"] = "¡Bienvenido a Estrella! Tu cuenta quedó creada.";
        return RedirectToAction("Index", "Perfil");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Salir()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    private async Task Entrar(Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Name, usuario.Nombre),
            new(ClaimTypes.Email, usuario.Correo)
        };
        var identidad = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identidad),
            new AuthenticationProperties { IsPersistent = true });
    }

    private IActionResult Redirigir(string? returnUrl)
    {
        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl)) return LocalRedirect(returnUrl);
        return RedirectToAction("Index", "Perfil");
    }
}
