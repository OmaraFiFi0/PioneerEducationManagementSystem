using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pioneer_Education.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditingCourseModuleEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Level",
                table: "Course",
                newName: "courseLevel");

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Course",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EnrolledStudents",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Course",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Course",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalStudents",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "EnrolledStudents",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "TotalStudents",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "courseLevel",
                table: "Course",
                newName: "Level");
        }
    }
}
