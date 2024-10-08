using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class addTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0ecf50c1-ea9d-4c7f-a9e6-aa5c7e65ed89");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b9762650-1fb3-42ef-8d1d-66189ab6ea45");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cebdc5cd-401e-4258-bd69-082f26437a80");

            migrationBuilder.CreateTable(
                name: "TransactionLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionLogs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "205d4496-4ac8-40d9-84b9-e09e1ada7a49", "acccef8b-20f3-4de0-8ee9-5a3690f094ed", "Human Resources", "Human Resources" },
                    { "4e7b2c09-e0b0-4ddd-9694-ebf3e21e2472", "1a777fbf-24db-4247-bd76-db376d703ea9", "User", "USER" },
                    { "83292e2c-6c86-4153-bdc5-760d05ec2293", "606fea67-ae89-4b3f-ac93-ccceda6fc85f", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionLogs");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "205d4496-4ac8-40d9-84b9-e09e1ada7a49");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e7b2c09-e0b0-4ddd-9694-ebf3e21e2472");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83292e2c-6c86-4153-bdc5-760d05ec2293");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0ecf50c1-ea9d-4c7f-a9e6-aa5c7e65ed89", "95c7a3bb-7b47-4702-91fd-27af6692a338", "Human Resources", "Human Resources" },
                    { "b9762650-1fb3-42ef-8d1d-66189ab6ea45", "97c8ebd9-8b58-4182-968b-870f2405b3d2", "Admin", "ADMIN" },
                    { "cebdc5cd-401e-4258-bd69-082f26437a80", "13b0ce7c-a231-427e-97a7-ae07d2c56eda", "User", "USER" }
                });
        }
    }
}
