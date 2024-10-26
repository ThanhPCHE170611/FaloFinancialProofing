using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class changeCreateProjectRequestName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreateProjectFiles_CreateProjects_RequestId",
                table: "CreateProjectFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateProjectRequestApproveHistories_CreateProjects_CreateProjectRequestId",
                table: "CreateProjectRequestApproveHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateProjects_Projects_ProjectId",
                table: "CreateProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateProjects_Users_ReceiverId",
                table: "CreateProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateProjects_Users_SenderId",
                table: "CreateProjects");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateProjects",
                table: "CreateProjects");

            migrationBuilder.RenameTable(
                name: "CreateProjects",
                newName: "CreateProjectRequests");

            migrationBuilder.RenameIndex(
                name: "IX_CreateProjects_SenderId",
                table: "CreateProjectRequests",
                newName: "IX_CreateProjectRequests_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateProjects_ReceiverId",
                table: "CreateProjectRequests",
                newName: "IX_CreateProjectRequests_ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateProjects_ProjectId",
                table: "CreateProjectRequests",
                newName: "IX_CreateProjectRequests_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateProjectRequests",
                table: "CreateProjectRequests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CreateProjectFiles_CreateProjectRequests_RequestId",
                table: "CreateProjectFiles",
                column: "RequestId",
                principalTable: "CreateProjectRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateProjectRequestApproveHistories_CreateProjectRequests_CreateProjectRequestId",
                table: "CreateProjectRequestApproveHistories",
                column: "CreateProjectRequestId",
                principalTable: "CreateProjectRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateProjectRequests_Projects_ProjectId",
                table: "CreateProjectRequests",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateProjectRequests_Users_ReceiverId",
                table: "CreateProjectRequests",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateProjectRequests_Users_SenderId",
                table: "CreateProjectRequests",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreateProjectFiles_CreateProjectRequests_RequestId",
                table: "CreateProjectFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateProjectRequestApproveHistories_CreateProjectRequests_CreateProjectRequestId",
                table: "CreateProjectRequestApproveHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateProjectRequests_Projects_ProjectId",
                table: "CreateProjectRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateProjectRequests_Users_ReceiverId",
                table: "CreateProjectRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_CreateProjectRequests_Users_SenderId",
                table: "CreateProjectRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CreateProjectRequests",
                table: "CreateProjectRequests");

            migrationBuilder.RenameTable(
                name: "CreateProjectRequests",
                newName: "CreateProjects");

            migrationBuilder.RenameIndex(
                name: "IX_CreateProjectRequests_SenderId",
                table: "CreateProjects",
                newName: "IX_CreateProjects_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateProjectRequests_ReceiverId",
                table: "CreateProjects",
                newName: "IX_CreateProjects_ReceiverId");

            migrationBuilder.RenameIndex(
                name: "IX_CreateProjectRequests_ProjectId",
                table: "CreateProjects",
                newName: "IX_CreateProjects_ProjectId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CreateProjects",
                table: "CreateProjects",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CreateProjectFiles_CreateProjects_RequestId",
                table: "CreateProjectFiles",
                column: "RequestId",
                principalTable: "CreateProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateProjectRequestApproveHistories_CreateProjects_CreateProjectRequestId",
                table: "CreateProjectRequestApproveHistories",
                column: "CreateProjectRequestId",
                principalTable: "CreateProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateProjects_Projects_ProjectId",
                table: "CreateProjects",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateProjects_Users_ReceiverId",
                table: "CreateProjects",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CreateProjects_Users_SenderId",
                table: "CreateProjects",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
