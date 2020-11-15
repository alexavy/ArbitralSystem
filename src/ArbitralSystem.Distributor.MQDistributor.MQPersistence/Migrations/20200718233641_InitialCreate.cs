using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "mqd");

            migrationBuilder.CreateTable(
                name: "Distributors",
                schema: "mqd",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<string>(unicode: false, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distributors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_Name_Type",
                schema: "mqd",
                table: "Distributors",
                columns: new[] { "Name", "Type" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Distributors",
                schema: "mqd");
        }
    }
}
