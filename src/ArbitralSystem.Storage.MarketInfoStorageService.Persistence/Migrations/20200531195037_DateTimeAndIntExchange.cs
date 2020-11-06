using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Persistence.Migrations
{
    public partial class DateTimeAndIntExchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Exchange",
                table: "OrderbookPriceEntries",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CatchAt",
                table: "OrderbookPriceEntries",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Exchange",
                table: "OrderbookPriceEntries",
                type: "varchar(max)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CatchAt",
                table: "OrderbookPriceEntries",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTime));
        }
    }
}
