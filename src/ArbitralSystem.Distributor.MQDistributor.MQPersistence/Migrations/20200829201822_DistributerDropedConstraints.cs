using Microsoft.EntityFrameworkCore.Migrations;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.Migrations
{
    public partial class DistributerDropedConstraints : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Distributors_Name_Type",
                schema: "mqd",
                table: "Distributors");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "mqd",
                table: "Distributors",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(900)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "mqd",
                table: "Distributors",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                schema: "mqd",
                table: "Distributors",
                type: "varchar(900)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "mqd",
                table: "Distributors",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Distributors_Name_Type",
                schema: "mqd",
                table: "Distributors",
                columns: new[] { "Name", "Type" },
                unique: true);
        }
    }
}
