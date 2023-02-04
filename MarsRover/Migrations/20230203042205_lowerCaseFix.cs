using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarsRover.Migrations
{
    /// <inheritdoc />
    public partial class lowerCaseFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "fDir",
                table: "Rover",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "pathData",
                table: "Rover",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pathData",
                table: "Rover");

            migrationBuilder.AlterColumn<string>(
                name: "fDir",
                table: "Rover",
                type: "nvarchar(1)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)");
        }
    }
}
