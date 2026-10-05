using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductosApi.Data;
using ProductosApi.Models;

namespace ProductosApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductosController(AppDbContext context)
    {
        _context = context;
    }

    // GET api/productos
    // Devuelve la lista completa de productos para el catálogo.
    // Público: no necesita iniciar sesión.
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
    {
        return await _context.Productos.ToListAsync();
    }

    // GET api/productos/5
    // Devuelve un solo producto por su id (lo usa la página de detalle).
    // Público: no necesita iniciar sesión. Responde 404 si no existe.
    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<Producto>> GetById(int id)
    {
        var producto = await _context.Productos.FindAsync(id);

        if (producto is null)
            return NotFound();

        return producto;
    }

    // POST api/productos
    // Crea un producto nuevo con los datos que manda el panel de administración.
    // Solo administradores.
    [HttpPost]
    [Authorize(Roles = "Admin")]
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

    // PUT api/productos/5
    // Edita un producto existente (nombre, descripción, precio, stock).
    // El id de la ruta debe coincidir con el id del cuerpo, si no responde 400.
    // Solo administradores.
    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
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

    // DELETE api/productos/5
    // Elimina un producto por su id. Responde 404 si no existe.
    // Solo administradores.
    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var producto = await _context.Productos.FindAsync(id);

        if (producto is null)
            return NotFound();

        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET api/productos/stock-mayor-1
    // Devuelve solo los productos que tienen más de 1 unidad en stock.
    // Público: es solo lectura.
    [HttpGet("stock-mayor-1")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Producto>>> GetStockMayorA1()
    {
        var productos = await _context.Productos
            .Where(p => p.Stock > 1)
            .ToListAsync();

        return productos;
    }

    // GET api/productos/precio-mayor-50
    // Devuelve solo los productos con precio mayor a 50.
    // Público: es solo lectura.
    [HttpGet("precio-mayor-50")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Producto>>> GetPrecioMayorA50()
    {
        var productos = await _context.Productos
            .Where(p => p.Precio > 50)
            .ToListAsync();

        return productos;
    }
}