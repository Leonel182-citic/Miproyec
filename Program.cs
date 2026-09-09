using Microsoft.EntityFrameworkCore;
using ProductosApi.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
 options.UseNpgsql(
 builder.Configuration.GetConnectionString("DefaultConnection")
 )
);
builder.Services.AddOpenApi();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
 app.MapOpenApi();
 app.UseSwagger();
 app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

