using Microsoft.EntityFrameworkCore.Migrations;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Persistence.Migrations
{
    public partial class NoIdOrderBookEntries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderbookPriceEntries",
                table: "OrderbookPriceEntries");

            migrationBuilder.DropColumn(name: "Id",
                table: "OrderbookPriceEntries");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "OrderbookPriceEntries",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long))
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderbookPriceEntries",
                table: "OrderbookPriceEntries",
                column: "Id");
        }
    }
}
