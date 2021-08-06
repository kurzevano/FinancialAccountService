using Microsoft.EntityFrameworkCore.Migrations;

namespace FinancialAccountService.Migrations
{
    public partial class AlterUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Balance_BalanceId",
                table: "User");

            migrationBuilder.AlterColumn<int>(
                name: "BalanceId",
                table: "User",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Balance_BalanceId",
                table: "User",
                column: "BalanceId",
                principalTable: "Balance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Balance_BalanceId",
                table: "User");

            migrationBuilder.AlterColumn<int>(
                name: "BalanceId",
                table: "User",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Balance_BalanceId",
                table: "User",
                column: "BalanceId",
                principalTable: "Balance",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
