using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.Migrations
{
    public partial class DistributorExchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exchanges",
                schema: "mqd",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exchanges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DistributorExchanges",
                schema: "mqd",
                columns: table => new
                {
                    DistributorId = table.Column<Guid>(nullable: false),
                    ExchangeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributorExchanges", x => new { x.DistributorId, x.ExchangeId });
                    table.ForeignKey(
                        name: "FK_DistributorExchanges_Distributors_DistributorId",
                        column: x => x.DistributorId,
                        principalSchema: "mqd",
                        principalTable: "Distributors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DistributorExchanges_Exchanges_ExchangeId",
                        column: x => x.ExchangeId,
                        principalSchema: "mqd",
                        principalTable: "Exchanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DistributorExchanges_ExchangeId",
                schema: "mqd",
                table: "DistributorExchanges",
                column: "ExchangeId");

            migrationBuilder.CreateIndex(
                name: "IX_Exchanges_Name",
                schema: "mqd",
                table: "Exchanges",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DistributorExchanges",
                schema: "mqd");

            migrationBuilder.DropTable(
                name: "Exchanges",
                schema: "mqd");
        }
    }
}
