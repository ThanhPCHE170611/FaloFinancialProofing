using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class addBirthDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1fe0c8b3-8edb-44fd-a5b5-22ef11374578");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8c8091bf-18a6-4944-91af-9d707e826b71");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fbc5c8c4-0654-44cd-b33f-2bdb3e7191cd");

            migrationBuilder.AddColumn<DateOnly>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "date",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "BirthDate",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1fe0c8b3-8edb-44fd-a5b5-22ef11374578", "277af920-1662-4299-90d6-ede1ae38ca70", "Admin", "ADMIN" },
                    { "8c8091bf-18a6-4944-91af-9d707e826b71", "bb421a79-babf-4407-bc63-1f1fdc7a48bf", "Human Resources", "Human Resources" },
                    { "fbc5c8c4-0654-44cd-b33f-2bdb3e7191cd", "c0c68286-8d18-40f8-9e5d-fbc210e17751", "User", "USER" }
                });
        }
    }
}
