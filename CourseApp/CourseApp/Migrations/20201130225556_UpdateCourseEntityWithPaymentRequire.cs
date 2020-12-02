using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseApp.Migrations
{
    public partial class UpdateCourseEntityWithPaymentRequire : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PaymentRequired",
                table: "Course",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "URL",
                table: "Course",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentRequired",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "URL",
                table: "Course");
        }
    }
}
