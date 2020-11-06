using Microsoft.EntityFrameworkCore.Migrations;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.Migrations
{
    public partial class StringStatusServer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                schema: "mqd",
                table: "Distributors",
                unicode: false,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Status",
                schema: "mqd",
                table: "Distributors",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false);
        }
    }
}
