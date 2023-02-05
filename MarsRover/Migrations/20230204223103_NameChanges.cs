using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarsRover.Migrations
{
    /// <inheritdoc />
    public partial class NameChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "input",
                table: "Rover",
                newName: "Input");

            migrationBuilder.RenameColumn(
                name: "sPosY",
                table: "Rover",
                newName: "StartingPositionY");

            migrationBuilder.RenameColumn(
                name: "sPosX",
                table: "Rover",
                newName: "StartingPositionX");

            migrationBuilder.RenameColumn(
                name: "sDir",
                table: "Rover",
                newName: "StartingDirection");

            migrationBuilder.RenameColumn(
                name: "pathData",
                table: "Rover",
                newName: "PlateauMap");

            migrationBuilder.RenameColumn(
                name: "fPosY",
                table: "Rover",
                newName: "FinalPositionY");

            migrationBuilder.RenameColumn(
                name: "fPosX",
                table: "Rover",
                newName: "FinalPositionX");

            migrationBuilder.RenameColumn(
                name: "fDir",
                table: "Rover",
                newName: "FinalDirection");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Input",
                table: "Rover",
                newName: "input");

            migrationBuilder.RenameColumn(
                name: "StartingPositionY",
                table: "Rover",
                newName: "sPosY");

            migrationBuilder.RenameColumn(
                name: "StartingPositionX",
                table: "Rover",
                newName: "sPosX");

            migrationBuilder.RenameColumn(
                name: "StartingDirection",
                table: "Rover",
                newName: "sDir");

            migrationBuilder.RenameColumn(
                name: "PlateauMap",
                table: "Rover",
                newName: "pathData");

            migrationBuilder.RenameColumn(
                name: "FinalPositionY",
                table: "Rover",
                newName: "fPosY");

            migrationBuilder.RenameColumn(
                name: "FinalPositionX",
                table: "Rover",
                newName: "fPosX");

            migrationBuilder.RenameColumn(
                name: "FinalDirection",
                table: "Rover",
                newName: "fDir");
        }
    }
}
