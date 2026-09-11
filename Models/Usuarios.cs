using System.ComponentModel.DataAnnotations;

namespace ProductosApi.Models;
public class Usuario
{
 public int Id { get; set; }
 public string Nombre { get; set; } = string.Empty;
 public string Primer_Apellido { get; set; } = string.Empty;
 public decimal Edad { get; set; }
 public string Correo { get; set; } = string.Empty;
 public string PasswordHash { get; set; } = string.Empty;
 public bool Activo { get; set;} = true;
 public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

}