using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseEventsApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUrlFieldInSourceEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_EventSourses_Url_EventId",
                table: "EventSourses",
                columns: new[] { "Url", "EventId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EventSourses_Url_EventId",
                table: "EventSourses");
        }
    }
}
