using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gp_backend.EF.MySql.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFileColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Messages",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Messages",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "Messages",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71c7d19b-93ef-42de-a43c-7a2178dd6d48",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "76d4b38c-9cb4-4b3f-a1b0-74915a0d63ec", "AQAAAAIAAYagAAAAEIFeLIODT4E7mKEU2m5f4DYae4aYeihIUInnA47wSeEYFZ67dTxxfzJPiBcWMFQk4w==", "3f02fdfb-c56f-4668-a9d9-0a5fca743ad7" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "Messages");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71c7d19b-93ef-42de-a43c-7a2178dd6d48",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ff622d98-508d-44a0-a1d9-cec8a8062ac4", "AQAAAAIAAYagAAAAEEu+rFkCbF7AX8WSLPh/rfuR87lbVzNmC+5wsZ7QlPITXsEN12LRPziHTXK5sVFmLw==", "7e401b3d-9390-4e0d-8c3c-11e35fe79f70" });
        }
    }
}
