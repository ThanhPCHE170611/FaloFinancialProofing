using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class addSocialNetworkAndCampaign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "056f8aca-62f1-4431-bbff-453c2cf01a6d");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "21d7ce15-d438-4ec7-b20d-f59655afe7b7");

            //migrationBuilder.DeleteData(
            //    table: "AspNetRoles",
            //    keyColumn: "Id",
            //    keyValue: "61370db3-90e6-4a4e-a3f1-f9a745f82087");

            migrationBuilder.CreateTable(
                name: "Campaign",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfCreation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TargetAmount = table.Column<decimal>(type: "money", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaign", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SocialNetworks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SocialNetworksLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialNetworks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocialNetworks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CampaignMember",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CampaignId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampaignMember", x => new { x.Id, x.UserId });
                    table.ForeignKey(
                        name: "FK_CampaignMember_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampaignMember_Campaign_Id",
                        column: x => x.Id,
                        principalTable: "Campaign",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.InsertData(
            //    table: "AspNetRoles",
            //    columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            //    values: new object[,]
            //    {
            //        { "32bb5397-afe7-4875-88e6-9382dba6f747", "c621a06d-14b9-49e5-9b73-3bfe9a75bbe6", "Admin", "ADMIN" },
            //        { "358dda90-1b69-4518-b4fd-6c1a14d2cfea", "88d4e430-4ae6-4fab-a059-50dfad33a9d7", "Human Resources", "Human Resources" },
            //        { "7217ef90-c670-46f1-869f-f01c9ed8e065", "ab4ab5a7-2b63-48e8-856a-af034407957a", "User", "USER" }
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_CampaignMember_UserId",
                table: "CampaignMember",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialNetworks_UserId",
                table: "SocialNetworks",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampaignMember");

            migrationBuilder.DropTable(
                name: "SocialNetworks");

            migrationBuilder.DropTable(
                name: "Campaign");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "32bb5397-afe7-4875-88e6-9382dba6f747");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "358dda90-1b69-4518-b4fd-6c1a14d2cfea");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7217ef90-c670-46f1-869f-f01c9ed8e065");

            //migrationBuilder.InsertData(
            //    table: "AspNetRoles",
            //    columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
            //    values: new object[,]
            //    {
            //        { "056f8aca-62f1-4431-bbff-453c2cf01a6d", "2c55592f-24a7-47d0-95ca-6c3c4b67c8fa", "User", "USER" },
            //        { "21d7ce15-d438-4ec7-b20d-f59655afe7b7", "0544c988-b90b-4bee-948a-7117b26ab303", "Admin", "ADMIN" },
            //        { "61370db3-90e6-4a4e-a3f1-f9a745f82087", "f7686586-9ed9-4007-82b5-9744a6b70a12", "Human Resources", "Human Resources" }
            //    });
        }
    }
}
