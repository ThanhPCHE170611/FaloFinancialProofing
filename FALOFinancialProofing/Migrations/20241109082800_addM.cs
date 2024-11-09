using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class addM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Bank_BankId",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateQrCode_Users_UserId",
                table: "CreateQrCode");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionLogs_CreateQrCode_CreateQrCodeId",
                table: "TransactionLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQrCode",
                table: "CreateQrCode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bank",
                table: "Bank");

            migrationBuilder.RenameTable(
                name: "CreateQrCode",
                newName: "CreateQrCodes");

            migrationBuilder.RenameTable(
                name: "Bank",
                newName: "Banks");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQrCode_UserId",
                table: "CreateQrCodes",
                newName: "IX_CreateQrCodes_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQrCodes",
                table: "CreateQrCodes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Banks",
                table: "Banks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Banks_BankId",
                table: "Campaigns",
                column: "BankId",
                principalTable: "Banks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQrCodes_Users_UserId",
                table: "CreateQrCodes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionLogs_CreateQrCodes_CreateQrCodeId",
                table: "TransactionLogs",
                column: "CreateQrCodeId",
                principalTable: "CreateQrCodes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Banks_BankId",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateQrCodes_Users_UserId",
                table: "CreateQrCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionLogs_CreateQrCodes_CreateQrCodeId",
                table: "TransactionLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateQrCodes",
                table: "CreateQrCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Banks",
                table: "Banks");

            migrationBuilder.RenameTable(
                name: "CreateQrCodes",
                newName: "CreateQrCode");

            migrationBuilder.RenameTable(
                name: "Banks",
                newName: "Bank");

            migrationBuilder.RenameIndex(
                name: "IX_CreateQrCodes_UserId",
                table: "CreateQrCode",
                newName: "IX_CreateQrCode_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateQrCode",
                table: "CreateQrCode",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bank",
                table: "Bank",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Bank_BankId",
                table: "Campaigns",
                column: "BankId",
                principalTable: "Bank",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CreateQrCode_Users_UserId",
                table: "CreateQrCode",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionLogs_CreateQrCode_CreateQrCodeId",
                table: "TransactionLogs",
                column: "CreateQrCodeId",
                principalTable: "CreateQrCode",
                principalColumn: "Id");
        }
    }
}
