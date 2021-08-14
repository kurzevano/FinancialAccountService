using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FinancialAccountService.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Balance",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Summ = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BalanceTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OperationTtype = table.Column<bool>(type: "INTEGER", nullable: false),
                    Summ = table.Column<decimal>(type: "TEXT", nullable: false),
                    BalanceId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalanceTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BalanceTransaction_Balance_BalanceId",
                        column: x => x.BalanceId,
                        principalTable: "Balance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LastName = table.Column<string>(type: "TEXT", nullable: true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: true),
                    MiddleName = table.Column<string>(type: "TEXT", nullable: true),
                    DateBirth = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BalanceId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Balance_BalanceId",
                        column: x => x.BalanceId,
                        principalTable: "Balance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BalanceTransaction_BalanceId",
                table: "BalanceTransaction",
                column: "BalanceId");

            migrationBuilder.CreateIndex(
                name: "IX_User_BalanceId",
                table: "User",
                column: "BalanceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BalanceTransaction");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Balance");
        }
    }
}
