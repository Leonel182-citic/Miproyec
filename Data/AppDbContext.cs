using Microsoft.EntityFrameworkCore;
using ProductosApi.Models;
namespace ProductosApi.Data;
public class AppDbContext : DbContext
{
 public AppDbContext(DbContextOptions<AppDbContext> options)
 : base(options)
 {
 }
 protected override void OnModelCreating(ModelBuilder modelBuilder)
{
 base.OnModelCreating(modelBuilder);
 modelBuilder.Entity<Usuario>()
 .HasIndex(u => u.Correo)
 .IsUnique();
}
 public DbSet<Producto> Productos => Set<Producto>();
 public DbSet<Usuario> Usuarios => Set<Usuario>();
}