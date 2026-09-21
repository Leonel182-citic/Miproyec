using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductosApi.Dtos.Auth;
using ProductosApi.Models;
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
    [HttpPut("actualizar/{id}")]
    public async Task<ActionResult> ActualizarUsuario(int id, Usuario usuario)
    {
        var contexto = _authService.GetType()
            .GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            .Select(campo => campo.GetValue(_authService))
            .FirstOrDefault(valor => valor?.GetType().GetProperty("Usuarios") != null);

        var usuarios = contexto?.GetType().GetProperty("Usuarios")?.GetValue(contexto) as IQueryable<Usuario>;
        var usuarioActualizado = usuarios == null
            ? null
            : await usuarios.FirstOrDefaultAsync(u => u.Id == id);

        if (usuarioActualizado == null)
        {
            return NotFound();
        }

        usuarioActualizado.Nombre = usuario.Nombre;
        usuarioActualizado.Primer_Apellido = usuario.Primer_Apellido;
        usuarioActualizado.Edad = usuario.Edad;
        usuarioActualizado.Correo = usuario.Correo;

        await _authService.SaveChangesAsync();
        return Ok(usuarioActualizado);
    }
}