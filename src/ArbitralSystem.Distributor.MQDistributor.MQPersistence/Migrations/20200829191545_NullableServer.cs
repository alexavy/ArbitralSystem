using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ArbitralSystem.Distributor.MQDistributor.MQPersistence.Migrations
{
    public partial class NullableServer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Distributors_Servers_ServerId",
                schema: "mqd",
                table: "Distributors");

            migrationBuilder.AlterColumn<Guid>(
                name: "ServerId",
                schema: "mqd",
                table: "Distributors",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Distributors_Servers_ServerId",
                schema: "mqd",
                table: "Distributors",
                column: "ServerId",
                principalSchema: "mqd",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Distributors_Servers_ServerId",
                schema: "mqd",
                table: "Distributors");

            migrationBuilder.AlterColumn<Guid>(
                name: "ServerId",
                schema: "mqd",
                table: "Distributors",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

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
    }
}
