using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseApp.Migrations
{
    public partial class AddSectionEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Section",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<long>(nullable: false),
                    ParentSectionId = table.Column<long>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Section", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Section_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Section_Section_ParentSectionId",
                        column: x => x.ParentSectionId,
                        principalTable: "Section",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Section_CourseId",
                table: "Section",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Section_ParentSectionId",
                table: "Section",
                column: "ParentSectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Section");
        }
    }
}
