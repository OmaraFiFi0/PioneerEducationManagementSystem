using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pioneer_Education.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "courseLevel",
                table: "Course",
                newName: "CourseLevel");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseLevel",
                table: "Course",
                newName: "courseLevel");
        }
    }
}
