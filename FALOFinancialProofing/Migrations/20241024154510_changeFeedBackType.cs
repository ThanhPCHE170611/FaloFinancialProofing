using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class changeFeedBackType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Feedback",
                table: "CreateProjects",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldMaxLength: 100);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Feedback",
                table: "CreateProjects",
                type: "datetime2",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
