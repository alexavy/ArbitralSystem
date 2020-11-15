using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.Migrations
{
    public partial class ServerAndDistributors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifyAt",
                schema: "mqd",
                table: "Distributors",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ServerId",
                schema: "mqd",
                table: "Distributors",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "mqd",
                table: "Distributors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Servers",
                schema: "mqd",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    ServerType = table.Column<string>(unicode: false, nullable: false),
                    MaxWorkersCount = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    ModifyAt = table.Column<DateTimeOffset>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_ServerId",
                schema: "mqd",
                table: "Distributors",
                column: "ServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Distributors_Servers_ServerId",
                schema: "mqd",
                table: "Distributors",
                column: "ServerId",
                principalSchema: "mqd",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Distributors_Servers_ServerId",
                schema: "mqd",
                table: "Distributors");

            migrationBuilder.DropTable(
                name: "Servers",
                schema: "mqd");

            migrationBuilder.DropIndex(
                name: "IX_Distributors_ServerId",
                schema: "mqd",
                table: "Distributors");

            migrationBuilder.DropColumn(
                name: "ModifyAt",
                schema: "mqd",
                table: "Distributors");

            migrationBuilder.DropColumn(
                name: "ServerId",
                schema: "mqd",
                table: "Distributors");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "mqd",
                table: "Distributors");
        }
    }
}
