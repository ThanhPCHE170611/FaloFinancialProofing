using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class addMoveNextCampaignStatusRequestModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatusOfCampaign",
                table: "MoveNextCampaignStatusRequests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MoveNextCampaignStatusRequestHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MoveNextCampaignStatusRequestId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateOfApproval = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAllowed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoveNextCampaignStatusRequestHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoveNextCampaignStatusRequestHistories_MoveNextCampaignStatusRequests_MoveNextCampaignStatusRequestId",
                        column: x => x.MoveNextCampaignStatusRequestId,
                        principalTable: "MoveNextCampaignStatusRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MoveNextCampaignStatusRequestHistories_Users_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoveNextCampaignStatusRequestHistories_MoveNextCampaignStatusRequestId",
                table: "MoveNextCampaignStatusRequestHistories",
                column: "MoveNextCampaignStatusRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_MoveNextCampaignStatusRequestHistories_ReceiverId",
                table: "MoveNextCampaignStatusRequestHistories",
                column: "ReceiverId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoveNextCampaignStatusRequestHistories");

            migrationBuilder.DropColumn(
                name: "StatusOfCampaign",
                table: "MoveNextCampaignStatusRequests");
        }
    }
}
