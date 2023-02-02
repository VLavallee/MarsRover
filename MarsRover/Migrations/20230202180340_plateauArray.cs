using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarsRover.Migrations
{
    /// <inheritdoc />
    public partial class plateauArray : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Input",
                table: "Rover",
                newName: "input");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "input",
                table: "Rover",
                newName: "Input");
        }
    }
}
