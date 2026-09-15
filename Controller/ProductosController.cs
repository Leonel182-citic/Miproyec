using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductosApi.Data;
using ProductosApi.Models;
namespace ProductosApi.Controllers;
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductosController : ControllerBase
{
 private readonly AppDbContext _context;
 public ProductosController(AppDbContext context)
 {
 _context = context;
 }
 [HttpGet]
 public async Task<ActionResult<IEnumerable<Producto>>> GetAll()
 {
 return await _context.Productos.ToListAsync();
 }
 [HttpGet("{id:int}")]
 public async Task<ActionResult<Producto>> GetById(int id)
 {
 var producto = await _context.Productos.FindAsync(id);
 if (producto is null)
 return NotFound();
 return producto;
 }
 [HttpPost]
 public async Task<ActionResult<Producto>> Create(Producto producto)
 {
 _context.Productos.Add(producto);
 await _context.SaveChangesAsync();
 return CreatedAtAction(
 nameof(GetById),
 new { id = producto.Id },
 producto
 );
 }
 [HttpPut("{id:int}")]
 public async Task<IActionResult> Update(int id, Producto producto)
 {
 if (id != producto.Id)
 return BadRequest();
 var existe = await _context.Productos.AnyAsync(p => p.Id == id);
 if (!existe)
 return NotFound();
 _context.Entry(producto).State = EntityState.Modified;
 await _context.SaveChangesAsync();
 return NoContent();
 }
 [HttpDelete("{id:int}")]
 public async Task<IActionResult> Delete(int id)
 {
 var producto = await _context.Productos.FindAsync(id);
 if (producto is null)
 return NotFound();
 _context.Productos.Remove(producto);
 await _context.SaveChangesAsync();
 return NoContent();
 }
 [HttpGet("stock-mayor-10")]
public async Task<ActionResult<IEnumerable<Producto>>> GetStockMayor10()
{
    var productos = await _context.Productos
        .Where(p => p.Stock > 10)
        .ToListAsync();

    return productos;
}
[HttpGet("precio-mayor-50")]
public async Task<ActionResult<IEnumerable<Producto>>> GetPrecioMayor1000()
{
    var productos = await _context.Productos
        .Where(p => p.Precio > 50)
        .ToListAsync();

    return productos;
}
}