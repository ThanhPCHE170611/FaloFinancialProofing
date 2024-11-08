using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class addm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreateQrCode_Users_UserId",
                table: "CreateQrCode");

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQrCode_Users_UserId",
                table: "CreateQrCode",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreateQrCode_Users_UserId",
                table: "CreateQrCode");

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQrCode_Users_UserId",
                table: "CreateQrCode",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
