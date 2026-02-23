using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eyedrop.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ghosts",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Scene = table.Column<string>(type: "TEXT", nullable: false),
                    Position = table.Column<string>(type: "TEXT", nullable: false),
                    LastSpeak = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ghosts", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Banned = table.Column<bool>(type: "INTEGER", nullable: false),
                    LastPlayed = table.Column<ulong>(type: "INTEGER", nullable: false),
                    Tokens = table.Column<string>(type: "TEXT", nullable: false),
                    LastSpawnData = table.Column<string>(type: "TEXT", nullable: false),
                    GhostName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Users_Ghosts_GhostName",
                        column: x => x.GhostName,
                        principalTable: "Ghosts",
                        principalColumn: "Name");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_GhostName",
                table: "Users",
                column: "GhostName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Ghosts");
        }
    }
}
