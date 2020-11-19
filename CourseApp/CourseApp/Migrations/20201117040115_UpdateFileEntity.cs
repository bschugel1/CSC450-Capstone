using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseApp.Migrations
{
    public partial class UpdateFileEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "MediaItems",
                newName: "Uri");

            migrationBuilder.AddColumn<string>(
                name: "MimeType",
                table: "MediaItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MimeType",
                table: "MediaItems");

            migrationBuilder.RenameColumn(
                name: "Uri",
                table: "MediaItems",
                newName: "Url");          
        }
    }
}
