using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArbitralSystem.PublicMarketInfoService.Persistence.Migrations
{
    public partial class PairPrices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PairPrices",
                columns: table => new
                {
                    ExchangePairName = table.Column<string>(type: "varchar(16)", maxLength: 12, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(19,9)", nullable: true),
                    Exchange = table.Column<int>(nullable: false),
                    Date = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PairPrices");
        }
    }
}
