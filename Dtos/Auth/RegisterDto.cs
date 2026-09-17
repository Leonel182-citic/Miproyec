using System.ComponentModel.DataAnnotations;
namespace ProductosApi.Dtos.Auth;
public class RegisterDto
{
 [Required]
 [MaxLength(100)]
 public string Nombre { get; set; } = string.Empty;
 [Required]
 [MaxLength(100)]
 public string Primer_Apellido { get; set; } = string.Empty;
 [Required]
 [MinLength(2)]
 public int Edad { get; set; }
 [Required]
 [EmailAddress]
 [MaxLength(150)]
 public string Correo { get; set; } = string.Empty;
 [Required]
 [MinLength(8)]
 public string Password { get; set; } = string.Empty;
}