using LavadoEstrella.Models;
using Microsoft.EntityFrameworkCore;

namespace LavadoEstrella.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opciones) : base(opciones) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Servicio> Servicios => Set<Servicio>();
    public DbSet<Empleado> Empleados => Set<Empleado>();
    public DbSet<Reserva> Reservas => Set<Reserva>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>().HasIndex(u => u.Correo).IsUnique();
    }
}
