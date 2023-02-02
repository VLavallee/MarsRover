using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarsRover.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rover",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sPosX = table.Column<int>(type: "int", nullable: false),
                    sPosY = table.Column<int>(type: "int", nullable: false),
                    sDir = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    Input = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    fPosX = table.Column<int>(type: "int", nullable: true),
                    fPosY = table.Column<int>(type: "int", nullable: true),
                    fDir = table.Column<string>(type: "nvarchar(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rover", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rover");
        }
    }
}
