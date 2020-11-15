using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArbitralSystem.PublicMarketInfoService.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PairInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ExchangePairName = table.Column<string>(nullable: true),
                    UnificatedPairName = table.Column<string>(nullable: true),
                    BaseCurrency = table.Column<string>(nullable: true),
                    QuoteCurrency = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    DelistedAt = table.Column<DateTimeOffset>(nullable: true),
                    Exchange = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PairInfos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PairInfos_ExchangePairName_Exchange_DelistedAt",
                table: "PairInfos",
                columns: new[] { "ExchangePairName", "Exchange", "DelistedAt" },
                unique: true,
                filter: "[ExchangePairName] IS NOT NULL AND [DelistedAt] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PairInfos");
        }
    }
}
