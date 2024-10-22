using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class addSeedRolee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "205d4496-4ac8-40d9-84b9-e09e1ada7a49",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Project Manager", "PROJECT MANAGER" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "4e7b2c09-e0b0-4ddd-9694-ebf3e21e2472",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Volunteer Leader", "VOLUNTEER LEADER" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "83292e2c-6c86-4153-bdc5-760d05ec2293",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Accounting", "ACCOUNTING" });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "83292e2c-6c86-4153-bdc5-760d05ec2295", "606fea67-ae89-4b3f-ac93-ccceda6fc85g", "Volunteer", "VOLUNTEER" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "83292e2c-6c86-4153-bdc5-760d05ec2295");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "205d4496-4ac8-40d9-84b9-e09e1ada7a49",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Admin", "ADMIN" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "4e7b2c09-e0b0-4ddd-9694-ebf3e21e2472",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "User", "USER" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: "83292e2c-6c86-4153-bdc5-760d05ec2293",
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Human Resources", "Human Resources" });
        }
    }
}
