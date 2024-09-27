using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class addSeedRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1fe0c8b3-8edb-44fd-a5b5-22ef11374578", "277af920-1662-4299-90d6-ede1ae38ca70", "Admin", "ADMIN" },
                    { "8c8091bf-18a6-4944-91af-9d707e826b71", "bb421a79-babf-4407-bc63-1f1fdc7a48bf", "Human Resources", "Human Resources" },
                    { "fbc5c8c4-0654-44cd-b33f-2bdb3e7191cd", "c0c68286-8d18-40f8-9e5d-fbc210e17751", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1fe0c8b3-8edb-44fd-a5b5-22ef11374578");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8c8091bf-18a6-4944-91af-9d707e826b71");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fbc5c8c4-0654-44cd-b33f-2bdb3e7191cd");
        }
    }
}
