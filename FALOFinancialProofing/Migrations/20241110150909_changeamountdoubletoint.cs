using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FALOFinancialProofing.Migrations
{
    /// <inheritdoc />
    public partial class changeamountdoubletoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "TransactionLogs",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "TransactionLogs",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
