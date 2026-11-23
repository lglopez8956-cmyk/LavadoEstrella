# Estrella · Lavado especializado (ASP.NET Core MVC)

Sitio web del negocio con: inicio con foto de fondo, servicios, reserva de citas, equipo,
promociones, contacto, cuenta de usuario y **Mi perfil con el historial de servicios**.

## Requisitos
- SDK de .NET 8 (https://dotnet.microsoft.com/download) o superior
- Visual Studio (carga de trabajo "ASP.NET y desarrollo web") o VS Code

## Cómo ejecutarlo
Visual Studio: abre `LavadoEstrella.csproj` y presiona F5.
Terminal:
```bash
dotnet restore
dotnet run
```
La primera vez se crea `estrella.db` (SQLite) con los servicios, el equipo y un usuario de prueba.
Para reiniciar los datos, borra `estrella.db` y vuelve a ejecutar.

**Usuario de prueba (ya tiene historial):** `demo@estrella.com` · contraseña `Demo1234`

## Qué hay en cada parte
| Pantalla | Archivo |
|---|---|
| Inicio (hero, servicios, reserva, equipo) | `Views/Home/Index.cshtml` |
| Servicios | `Views/Servicios/Index.cshtml`, `Views/Shared/_Servicios.cshtml` |
| Nosotros / Promociones / Contacto | `Views/Home/` |
| Reservar | `Controllers/ReservasController.cs`, `Views/Reservas/Crear.cshtml` |
| Ingresar / Registrarse | `Controllers/CuentaController.cs`, `Views/Cuenta/` |
| Mi perfil + historial | `Controllers/PerfilController.cs`, `Views/Perfil/Index.cshtml` |

## Cosas que debes editar
- **Teléfono, WhatsApp, Facebook, dirección y horario:** `appsettings.json`, sección `Negocio`.
- **Servicios, precios y equipo:** `Data/SeedData.cs` (borra `estrella.db` después de cambiarlos).
- **Promociones:** `Views/Home/Promociones.cshtml` (son ejemplos).
- **Imágenes:** `wwwroot/img/`. Las fotos y el logo fueron recortados del diseño; reemplázalos por
  los originales en mayor resolución usando los mismos nombres (`hero.jpg`, `logo.png`, `servicio-*.jpg`, `equipo-*.jpg`).

## Reglas de reserva
- Horario de 8:00 a 17:00, por horas (`Models/Entidades.cs`, clase `Agenda`).
- Máximo 2 reservas por hora (`Agenda.CupoPorHora`).
- Para reservar hay que iniciar sesión; después de ingresar se vuelve al formulario.
- Las reservas cuya fecha ya pasó pasan solas a "Realizado" al abrir Mi perfil.
