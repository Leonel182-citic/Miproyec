using System.ComponentModel.DataAnnotations;
namespace ProductosApi.Dtos.Auth;
public class LoginDto
{
 [Required]
 [EmailAddress]
 public string Correo { get; set; } = string.Empty;
 [Required]
 public string Password { get; set; } = string.Empty;
}
