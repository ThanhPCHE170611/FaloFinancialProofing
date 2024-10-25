using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class updateMoveNextCampaign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoveNextCampaignStatusRequests_Users_ReceiverId",
                table: "MoveNextCampaignStatusRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_MoveNextCampaignStatusRequests_Users_SenderId",
                table: "MoveNextCampaignStatusRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_MoveNextCampaignStatusRequests_Users_ReceiverId",
                table: "MoveNextCampaignStatusRequests",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MoveNextCampaignStatusRequests_Users_SenderId",
                table: "MoveNextCampaignStatusRequests",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MoveNextCampaignStatusRequests_Users_ReceiverId",
                table: "MoveNextCampaignStatusRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_MoveNextCampaignStatusRequests_Users_SenderId",
                table: "MoveNextCampaignStatusRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_MoveNextCampaignStatusRequests_Users_ReceiverId",
                table: "MoveNextCampaignStatusRequests",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MoveNextCampaignStatusRequests_Users_SenderId",
                table: "MoveNextCampaignStatusRequests",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
