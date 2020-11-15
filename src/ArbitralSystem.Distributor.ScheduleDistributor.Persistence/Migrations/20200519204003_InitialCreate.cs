using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArbitralSystem.Distributor.ScheduleDistributor.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Distributors",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DistributorType = table.Column<string>(nullable: true),
                    ServerName = table.Column<string>(nullable: true),
                    QueueName = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distributors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_Name_DistributorType",
                table: "Distributors",
                columns: new[] { "Name", "DistributorType" },
                unique: true,
                filter: "[Name] IS NOT NULL AND [DistributorType] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Distributors");
        }
    }
}
