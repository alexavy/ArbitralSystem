using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DistributerStates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Symbol = table.Column<string>(nullable: true),
                    Exchange = table.Column<string>(unicode: false, nullable: false),
                    ChangedAt = table.Column<DateTimeOffset>(nullable: false),
                    PreviousStatus = table.Column<int>(nullable: false),
                    CurrentStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributerStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderbookPriceEntries",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "varchar(16)", maxLength: 12, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(19,9)", nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(19,9)", nullable: false),
                    Exchange = table.Column<string>(unicode: false, nullable: false),
                    Direction = table.Column<byte>(type: "tinyint", nullable: false),
                    CatchAt = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderbookPriceEntries", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DistributerStates");

            migrationBuilder.DropTable(
                name: "OrderbookPriceEntries");
        }
    }
}
