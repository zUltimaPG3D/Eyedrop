using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eyedrop.Migrations.ADU
{
    /// <inheritdoc />
    public partial class FixUploadPaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TiedPath",
                table: "Uploads",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TiedPath",
                table: "Uploads");
        }
    }
}
