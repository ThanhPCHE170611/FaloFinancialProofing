using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class CreateProjectRequestApproveHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CreateProjectRequestApproveHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateProjectRequestId = table.Column<int>(type: "int", nullable: false),
                    ApproverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateOfApproval = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateProjectRequestApproveHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreateProjectRequestApproveHistories_CreateProjects_CreateProjectRequestId",
                        column: x => x.CreateProjectRequestId,
                        principalTable: "CreateProjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreateProjectRequestApproveHistories_Users_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreateProjectRequestApproveHistories_ApproverId",
                table: "CreateProjectRequestApproveHistories",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_CreateProjectRequestApproveHistories_CreateProjectRequestId",
                table: "CreateProjectRequestApproveHistories",
                column: "CreateProjectRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreateProjectRequestApproveHistories");
        }
    }
}
