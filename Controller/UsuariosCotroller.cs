using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductosApi.Data;
using ProductosApi.Models;
namespace ProductosApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class    UsuariosController : ControllerBase
{
 private readonly AppDbContext _context;
 public UsuariosController(AppDbContext context)
 {
 _context = context;
 }
 [HttpGet]
 public async Task<ActionResult<IEnumerable<Usuario>>> GetAll()
 {
 return await _context.Usuarios.ToListAsync();
 }
 [HttpGet("{id:int}")]
 public async Task<ActionResult<Usuario>> GetById(int id)
 {
 var usuario = await _context.Usuarios.FindAsync(id);
 if (usuario is null)
 return NotFound();
 return usuario;
 }
 [HttpPost]
 public async Task<ActionResult<Usuario>> Create(Usuario usuario)
 {
 _context.Usuarios.Add(usuario);
 await _context.SaveChangesAsync();
 return CreatedAtAction(
 nameof(GetById),
 new { id = usuario.Id },
 usuario
 );
 }
 [HttpPut("{id:int}")]
 public async Task<IActionResult> Update(int id, Usuario usuario)
 {
 if (id != usuario.Id)
 return BadRequest();
 var existe = await _context.Usuarios.AnyAsync(p => p.Id == id);
 if (!existe)
 return NotFound();
 _context.Entry(usuario).State = EntityState.Modified;
 await _context.SaveChangesAsync();
 return NoContent();
 }
 [HttpDelete("{id:int}")]
 public async Task<IActionResult> Delete(int id)
 {
 var usuario = await _context.Usuarios.FindAsync(id);
 if (usuario is null)
 return NotFound();
 _context.Usuarios.Remove(usuario);
 await _context.SaveChangesAsync();
 return NoContent();
 }
}