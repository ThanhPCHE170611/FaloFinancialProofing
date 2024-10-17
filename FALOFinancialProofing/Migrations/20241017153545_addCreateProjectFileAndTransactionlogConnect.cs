using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class addCreateProjectFileAndTransactionlogConnect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreateProjectFiles_CreateProjects_ProjectRequestId",
                table: "CreateProjectFiles");

            migrationBuilder.DropIndex(
                name: "IX_CreateProjectFiles_ProjectRequestId",
                table: "CreateProjectFiles");

            migrationBuilder.DropColumn(
                name: "ProjectRequestId",
                table: "CreateProjectFiles");

            migrationBuilder.AlterColumn<string>(
                name: "SenderID",
                table: "TransactionLogs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionLogs_SenderID",
                table: "TransactionLogs",
                column: "SenderID");

            migrationBuilder.CreateIndex(
                name: "IX_CreateProjectFiles_RequestId",
                table: "CreateProjectFiles",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_CreateProjectFiles_CreateProjects_RequestId",
                table: "CreateProjectFiles",
                column: "RequestId",
                principalTable: "CreateProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionLogs_AspNetUsers_SenderID",
                table: "TransactionLogs",
                column: "SenderID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreateProjectFiles_CreateProjects_RequestId",
                table: "CreateProjectFiles");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionLogs_AspNetUsers_SenderID",
                table: "TransactionLogs");

            migrationBuilder.DropIndex(
                name: "IX_TransactionLogs_SenderID",
                table: "TransactionLogs");

            migrationBuilder.DropIndex(
                name: "IX_CreateProjectFiles_RequestId",
                table: "CreateProjectFiles");

            migrationBuilder.AlterColumn<string>(
                name: "SenderID",
                table: "TransactionLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "ProjectRequestId",
                table: "CreateProjectFiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CreateProjectFiles_ProjectRequestId",
                table: "CreateProjectFiles",
                column: "ProjectRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_CreateProjectFiles_CreateProjects_ProjectRequestId",
                table: "CreateProjectFiles",
                column: "ProjectRequestId",
                principalTable: "CreateProjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
