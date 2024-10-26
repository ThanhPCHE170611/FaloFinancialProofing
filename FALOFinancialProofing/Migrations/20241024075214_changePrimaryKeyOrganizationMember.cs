using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class changePrimaryKeyOrganizationMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationMember",
                table: "OrganizationMember");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "OrganizationMember",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationMember",
                table: "OrganizationMember",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationMember_UserId",
                table: "OrganizationMember",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrganizationMember",
                table: "OrganizationMember");

            migrationBuilder.DropIndex(
                name: "IX_OrganizationMember_UserId",
                table: "OrganizationMember");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "OrganizationMember");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrganizationMember",
                table: "OrganizationMember",
                columns: new[] { "UserId", "OrganizationId" });
        }
    }
}
