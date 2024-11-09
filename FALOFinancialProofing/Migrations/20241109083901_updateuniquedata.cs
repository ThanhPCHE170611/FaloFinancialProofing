using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class updateuniquedata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TransactionLogs_CassoTransactionId",
                table: "TransactionLogs",
                column: "CassoTransactionId",
                unique: true,
                filter: "[CassoTransactionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Banks_CassoAccountID",
                table: "Banks",
                column: "CassoAccountID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransactionLogs_CassoTransactionId",
                table: "TransactionLogs");

            migrationBuilder.DropIndex(
                name: "IX_Banks_CassoAccountID",
                table: "Banks");
        }
    }
}
