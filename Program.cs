using Microsoft.EntityFrameworkCore;
using ProductosApi.Data;
using Microsoft.AspNetCore.Identity;
using ProductosApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ProductosApi.Services;
using Microsoft.OpenApi;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSwaggerGen(options =>
{
 options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
 {
 Type = SecuritySchemeType.Http,
 Scheme = "bearer",
 BearerFormat = "JWT",
 Description = "JWT Authorization header using the Bearer scheme."
 });
 options.AddSecurityRequirement(document =>
 new OpenApiSecurityRequirement
 {
 [new OpenApiSecuritySchemeReference(
 "bearer",
 document
 )] = []
 });
});
builder.Services.AddControllers();
builder.Services.AddScoped<AuthService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
 options.UseNpgsql(
 builder.Configuration.GetConnectionString("DefaultConnection")
 )
);
builder.Services.AddOpenApi();
var jwtKey = builder.Configuration["Jwt:Key"]
 ?? throw new InvalidOperationException("Jwt:Key no está configurado.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];
builder.Services
 .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
 options.TokenValidationParameters = new TokenValidationParameters
 {
 ValidateIssuer = true,
 ValidateAudience = true,
 ValidateLifetime = true,
 ValidateIssuerSigningKey = true,
 ValidIssuer = jwtIssuer,
 ValidAudience = jwtAudience,
 IssuerSigningKey = new SymmetricSecurityKey(
 Encoding.UTF8.GetBytes(jwtKey)
 ),
 ClockSkew = TimeSpan.Zero
 };
 });
builder.Services.AddAuthorization();
builder.Services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
 app.MapOpenApi();
 app.UseSwagger();
 app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

