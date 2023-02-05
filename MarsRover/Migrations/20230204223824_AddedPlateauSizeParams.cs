using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarsRover.Migrations
{
    /// <inheritdoc />
    public partial class AddedPlateauSizeParams : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlateauSizeX",
                table: "Rover",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlateauSizeY",
                table: "Rover",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlateauSizeX",
                table: "Rover");

            migrationBuilder.DropColumn(
                name: "PlateauSizeY",
                table: "Rover");
        }
    }
}
