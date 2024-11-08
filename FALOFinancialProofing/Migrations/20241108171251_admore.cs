using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class admore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BankId",
                table: "TransactionLogs",
                newName: "tid");

            migrationBuilder.AddColumn<long>(
                name: "CassoTransactionId",
                table: "TransactionLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreateQrCodeId",
                table: "TransactionLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BankCodeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    acqId = table.Column<int>(type: "int", nullable: false),
                    CassoAccountID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreateQrCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateQrCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreateQrCode_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLogs_CreateQrCodeId",
                table: "TransactionLogs",
                column: "CreateQrCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Campaigns_BankId",
                table: "Campaigns",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateQrCode_UserId",
                table: "CreateQrCode",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_Bank_BankId",
                table: "Campaigns",
                column: "BankId",
                principalTable: "Bank",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionLogs_CreateQrCode_CreateQrCodeId",
                table: "TransactionLogs",
                column: "CreateQrCodeId",
                principalTable: "CreateQrCode",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_Bank_BankId",
                table: "Campaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionLogs_CreateQrCode_CreateQrCodeId",
                table: "TransactionLogs");

            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.DropTable(
                name: "CreateQrCode");

            migrationBuilder.DropIndex(
                name: "IX_TransactionLogs_CreateQrCodeId",
                table: "TransactionLogs");

            migrationBuilder.DropIndex(
                name: "IX_Campaigns_BankId",
                table: "Campaigns");

            migrationBuilder.DropColumn(
                name: "CassoTransactionId",
                table: "TransactionLogs");

            migrationBuilder.DropColumn(
                name: "CreateQrCodeId",
                table: "TransactionLogs");

            migrationBuilder.RenameColumn(
                name: "tid",
                table: "TransactionLogs",
                newName: "BankId");
        }
    }
}
