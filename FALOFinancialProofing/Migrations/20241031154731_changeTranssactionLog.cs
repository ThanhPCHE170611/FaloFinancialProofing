using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class changeTranssactionLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionLogs_Users_SenderID",
                table: "TransactionLogs");

            migrationBuilder.DropIndex(
                name: "IX_TransactionLogs_SenderID",
                table: "TransactionLogs");

            migrationBuilder.DropColumn(
                name: "SenderID",
                table: "TransactionLogs");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLogs_CampaignId",
                table: "TransactionLogs",
                column: "CampaignId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionLogs_Campaigns_CampaignId",
                table: "TransactionLogs",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionLogs_Campaigns_CampaignId",
                table: "TransactionLogs");

            migrationBuilder.DropIndex(
                name: "IX_TransactionLogs_CampaignId",
                table: "TransactionLogs");

            migrationBuilder.AddColumn<string>(
                name: "SenderID",
                table: "TransactionLogs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLogs_SenderID",
                table: "TransactionLogs",
                column: "SenderID");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionLogs_Users_SenderID",
                table: "TransactionLogs",
                column: "SenderID",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
