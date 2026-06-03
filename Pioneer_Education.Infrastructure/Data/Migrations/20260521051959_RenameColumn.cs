using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pioneer_Education.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalStudents",
                table: "Course",
                newName: "FreeSpaceInCourse");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FreeSpaceInCourse",
                table: "Course",
                newName: "TotalStudents");
        }
    }
}
