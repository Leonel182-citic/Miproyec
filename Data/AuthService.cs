using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProductosApi.Data;
using ProductosApi.Dtos.Auth;
using ProductosApi.Models;
namespace ProductosApi.Services;
public class AuthService
{
 private readonly AppDbContext _context;
 private readonly IPasswordHasher<Usuario> _passwordHasher;
 private readonly IConfiguration _configuration;
 public AuthService(
 AppDbContext context,
 IPasswordHasher<Usuario> passwordHasher,
 IConfiguration configuration)
 {
 _context = context;
 _passwordHasher = passwordHasher;
 _configuration = configuration;
 }
 public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
 {
 var correo = dto.Correo.Trim().ToLower();
 var existe = await _context.Usuarios
 .AnyAsync(u => u.Correo == correo);
 if (existe)
 return null;
 var usuario = new Usuario
 {
 Nombre = dto.Nombre.Trim(),
 Primer_Apellido = dto.Primer_Apellido.Trim(),
 Edad = dto.Edad,
 Correo = correo,
 Activo = true
 };
 usuario.PasswordHash =
 _passwordHasher.HashPassword(usuario, dto.Password);
 _context.Usuarios.Add(usuario);
 await _context.SaveChangesAsync();
 return CrearRespuesta(usuario);
 }
 public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
 {
 var correo = dto.Correo.Trim().ToLower();
 var usuario = await _context.Usuarios
 .FirstOrDefaultAsync(u => u.Correo == correo);
 if (usuario is null || !usuario.Activo)
 return null;
 var resultado = _passwordHasher.VerifyHashedPassword(
 usuario,
 usuario.PasswordHash,
 dto.Password
 );
 if (resultado == PasswordVerificationResult.Failed)
 return null;
 return CrearRespuesta(usuario);
 }
 private AuthResponseDto CrearRespuesta(Usuario usuario)
 {
 var expiracionMinutos =
 _configuration.GetValue<int?>("Jwt:ExpirationMinutes") ?? 60;
 var expiraEn = DateTime.UtcNow.AddMinutes(expiracionMinutos);
 var claims = new List<Claim>
 {
 new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
 new Claim(ClaimTypes.Name, usuario.Nombre),
 new Claim("Primer_Apellido", usuario.Primer_Apellido),
 new Claim("Edad", usuario.Edad.ToString()),
 new Claim(ClaimTypes.Email, usuario.Correo)
 };
 var key = new SymmetricSecurityKey(
 Encoding.UTF8.GetBytes(
 _configuration["Jwt:Key"]!
 )
 );
 var credentials = new SigningCredentials(
 key,
 SecurityAlgorithms.HmacSha256
 );
 var token = new JwtSecurityToken(
 issuer: _configuration["Jwt:Issuer"],
 audience: _configuration["Jwt:Audience"],
 claims: claims,
 expires: expiraEn,
 signingCredentials: credentials
 );
 var tokenString =
 new JwtSecurityTokenHandler().WriteToken(token);
 return new AuthResponseDto
 {
 Token = tokenString,
 ExpiraEn = expiraEn,
 UsuarioId = usuario.Id,
 Nombre = usuario.Nombre,
 Correo = usuario.Correo
 };
 }
}
