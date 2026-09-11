namespace ProductosApi.Dtos.Auth;
public class AuthResponseDto
{
 public string Token { get; set; } = string.Empty;
 public DateTime ExpiraEn { get; set; }
 public int UsuarioId { get; set; }
 public string Nombre { get; set; } = string.Empty;
 public string Correo { get; set; } = string.Empty;
}
