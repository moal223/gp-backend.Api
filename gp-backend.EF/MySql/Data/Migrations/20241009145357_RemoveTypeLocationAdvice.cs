using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gp_backend.EF.MySql.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTypeLocationAdvice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Advice",
                table: "Wounds");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Wounds");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Wounds");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71c7d19b-93ef-42de-a43c-7a2178dd6d48",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "232932e5-ed96-41ca-84e2-79a4babefbe3", "AQAAAAIAAYagAAAAECTZkJBsP2GZqZXOOKimee3M2H9YTPndigZr6rM0Fwm4CawbTd4Qg8/0pThHF+lFzA==", "2ab44f8e-da22-4020-a1f9-4362f2e48259" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Advice",
                table: "Wounds",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Wounds",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Wounds",
                type: "varchar(150)",
                maxLength: 150,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "71c7d19b-93ef-42de-a43c-7a2178dd6d48",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "65492912-f68d-47ed-ad83-76d39b7aa1a1", "AQAAAAIAAYagAAAAEC+3C1cGGA5L1oxUqJCENsQ/ttjABBshds55due1gkkY4k8rneB3gf6QNLo4x7qIkA==", "3ce09e50-e57a-49fd-9231-a547e74a6214" });
        }
    }
}
