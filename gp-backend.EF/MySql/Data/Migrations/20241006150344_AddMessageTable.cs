using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gp_backend.EF.MySql.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SenderId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RecipientId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Timestamp = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71c7d19b-93ef-42de-a43c-7a2178dd6d48",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "65492912-f68d-47ed-ad83-76d39b7aa1a1", "AQAAAAIAAYagAAAAEC+3C1cGGA5L1oxUqJCENsQ/ttjABBshds55due1gkkY4k8rneB3gf6QNLo4x7qIkA==", "3ce09e50-e57a-49fd-9231-a547e74a6214" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71c7d19b-93ef-42de-a43c-7a2178dd6d48",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a0270b3d-15be-4846-aa87-b2126488b0a9", "AQAAAAIAAYagAAAAEB8hJ2bBccU3iidiBUvecj/aeZNHnQ5Q/y2cjsn2dMyFuOZJ2O5Ux9dBWFiaxDsgtw==", "a7f54610-b943-45e7-b616-7598180f4fb1" });
        }
    }
}
