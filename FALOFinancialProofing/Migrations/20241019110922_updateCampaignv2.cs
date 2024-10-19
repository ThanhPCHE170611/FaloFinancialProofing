using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class updateCampaignv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Campaign_AspNetUsers_CreateBy",
                table: "Campaign");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignMember_AspNetUsers_UserId",
                table: "CampaignMember");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignMember_Campaign_CampaignId",
                table: "CampaignMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CampaignMember",
                table: "CampaignMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Campaign",
                table: "Campaign");

            migrationBuilder.RenameTable(
                name: "CampaignMember",
                newName: "CampaignMembers");

            migrationBuilder.RenameTable(
                name: "Campaign",
                newName: "Campaigns");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignMember_UserId",
                table: "CampaignMembers",
                newName: "IX_CampaignMembers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Campaign_CreateBy",
                table: "Campaigns",
                newName: "IX_Campaigns_CreateBy");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CampaignMembers",
                table: "CampaignMembers",
                columns: new[] { "CampaignId", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Campaigns",
                table: "Campaigns",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignMembers_AspNetUsers_UserId",
                table: "CampaignMembers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignMembers_Campaigns_CampaignId",
                table: "CampaignMembers",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Campaigns_AspNetUsers_CreateBy",
                table: "Campaigns",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CampaignMembers_AspNetUsers_UserId",
                table: "CampaignMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_CampaignMembers_Campaigns_CampaignId",
                table: "CampaignMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Campaigns_AspNetUsers_CreateBy",
                table: "Campaigns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Campaigns",
                table: "Campaigns");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CampaignMembers",
                table: "CampaignMembers");

            migrationBuilder.RenameTable(
                name: "Campaigns",
                newName: "Campaign");

            migrationBuilder.RenameTable(
                name: "CampaignMembers",
                newName: "CampaignMember");

            migrationBuilder.RenameIndex(
                name: "IX_Campaigns_CreateBy",
                table: "Campaign",
                newName: "IX_Campaign_CreateBy");

            migrationBuilder.RenameIndex(
                name: "IX_CampaignMembers_UserId",
                table: "CampaignMember",
                newName: "IX_CampaignMember_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Campaign",
                table: "Campaign",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CampaignMember",
                table: "CampaignMember",
                columns: new[] { "CampaignId", "UserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Campaign_AspNetUsers_CreateBy",
                table: "Campaign",
                column: "CreateBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignMember_AspNetUsers_UserId",
                table: "CampaignMember",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CampaignMember_Campaign_CampaignId",
                table: "CampaignMember",
                column: "CampaignId",
                principalTable: "Campaign",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
