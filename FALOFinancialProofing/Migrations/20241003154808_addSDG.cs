using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class addSDG : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "SDG",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SDGName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SDG", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SDG_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1a76f1e3-04cd-41a3-9058-ff28c95dd6f6", "7673d91e-ea44-4d50-ba6e-870443eeaa9d", "Human Resources", "Human Resources" },
                    { "4eec9535-5bc1-494a-a770-878c47d726cb", "c6fb007f-2050-4a0a-9513-d791d3730a18", "User", "USER" },
                    { "cc9a00dd-d0d8-4569-981d-1bb1bc25327b", "f80c903e-6c98-47ca-a9a0-728120f32255", "Admin", "ADMIN" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SDG_UserId",
                table: "SDG",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SDG");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1a76f1e3-04cd-41a3-9058-ff28c95dd6f6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4eec9535-5bc1-494a-a770-878c47d726cb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cc9a00dd-d0d8-4569-981d-1bb1bc25327b");

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
    }
}
