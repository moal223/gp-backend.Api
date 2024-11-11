using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gp_backend.EF.MySql.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFileColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Images_FileId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_FileId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Messages");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71c7d19b-93ef-42de-a43c-7a2178dd6d48",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ff622d98-508d-44a0-a1d9-cec8a8062ac4", "AQAAAAIAAYagAAAAEEu+rFkCbF7AX8WSLPh/rfuR87lbVzNmC+5wsZ7QlPITXsEN12LRPziHTXK5sVFmLw==", "7e401b3d-9390-4e0d-8c3c-11e35fe79f70" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71c7d19b-93ef-42de-a43c-7a2178dd6d48",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a867ae25-5166-4272-8a53-521b58dd50bf", "AQAAAAIAAYagAAAAECZuLnk+0Zyc/nO+Y83M2okXyAqMcgDyw59hVfsDWKC/BjtjfH9F4/s5EQUYDk7EJA==", "363a5cc7-6035-4216-abef-530737b0c52a" });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_FileId",
                table: "Messages",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Images_FileId",
                table: "Messages",
                column: "FileId",
                principalTable: "Images",
                principalColumn: "Id");
        }
    }
}
