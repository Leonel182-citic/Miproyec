using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductosApi.Migrations
{
    /// <inheritdoc />
    public partial class AddTiendaProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tienda",
                table: "Productos",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tienda",
                table: "Productos");
        }
    }
}
