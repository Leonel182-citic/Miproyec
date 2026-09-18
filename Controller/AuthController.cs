using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductosApi.Dtos.Auth;
using ProductosApi.Services;
namespace ProductosApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
 private readonly AuthService _authService;
 public AuthController(AuthService authService)
 {
 _authService = authService;
 }
 [HttpPost("register")]
 public async Task<ActionResult<AuthResponseDto>> Register(
 RegisterDto dto)
 {
 var resultado = await _authService.RegisterAsync(dto);
 if (resultado is null)
 return Conflict(new
 {
 message = "Ya existe un usuario con ese correo."
 });
 return Ok(resultado);
 }
 [HttpPost("login")]
 public async Task<ActionResult<AuthResponseDto>> Login(
 LoginDto dto)
 {
 var resultado = await _authService.LoginAsync(dto);
 if (resultado is null)
 return Unauthorized(new
 {
 message = "Correo o contraseña incorrectos."
 });
 return Ok(resultado);
 }
 [Authorize]
 [HttpGet("me")]
 public IActionResult Me()
 {
 var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
 var nombre = User.FindFirstValue(ClaimTypes.Name);
 var Primer_Apellido = User.FindFirstValue(ClaimTypes.Name);
 var Edadtexto = User.FindFirstValue("Edad");
 var correo = User.FindFirstValue(ClaimTypes.Email);
 int.TryParse(Edadtexto, out var Edad);
 return Ok(new
 {
 id,
 nombre,
 Primer_Apellido,
 Edad,
 correo
 });
    }
[HttpPut("profile")]
[Authorize]
public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto)
{
    var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    var resultado = await _authService.UpdateProfileAsync(usuarioId, dto);

    if (resultado is null)
        return NotFound("Usuario no encontrado o inactivo.");

    return Ok(resultado);
}
}

public class UpdateProfileDto
{
    public object? Nombre { get; internal set; }
    public object? Primer_Apellido { get; internal set; }
}