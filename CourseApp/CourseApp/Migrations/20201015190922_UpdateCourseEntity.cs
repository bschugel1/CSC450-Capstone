using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CourseApp.Migrations
{
    public partial class UpdateCourseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CloudId",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Course");

            migrationBuilder.AddColumn<long>(
                name: "AuthorId",
                table: "Course",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "CourseCode",
                table: "Course",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Course_AuthorId",
                table: "Course",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_AspNetUsers_AuthorId",
                table: "Course",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_AspNetUsers_AuthorId",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_AuthorId",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "CourseCode",
                table: "Course");

            migrationBuilder.AddColumn<Guid>(
                name: "CloudId",
                table: "Course",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "CourseId",
                table: "Course",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
