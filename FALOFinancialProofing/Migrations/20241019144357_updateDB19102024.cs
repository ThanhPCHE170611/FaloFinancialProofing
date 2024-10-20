using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class updateDB19102024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampaignMember_Campaigns_Id",
                table: "CampaignMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CampaignMember",
                table: "CampaignMember");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CampaignMember",
                table: "CampaignMember",
                columns: new[] { "CampaignId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignMember_Campaigns_CampaignId",
                table: "CampaignMember",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampaignMember_Campaigns_CampaignId",
                table: "CampaignMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CampaignMember",
                table: "CampaignMember");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CampaignMember",
                table: "CampaignMember",
                columns: new[] { "Id", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignMember_Campaigns_Id",
                table: "CampaignMember",
                column: "Id",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
