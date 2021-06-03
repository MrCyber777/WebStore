using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebStore.Data.Migrations
{
    public partial class ProductsForAppointmentChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductsForAppointments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ProductsForAppointments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductsForAppointments_UserId",
                table: "ProductsForAppointments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsForAppointments_AspNetUsers_UserId",
                table: "ProductsForAppointments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsForAppointments_AspNetUsers_UserId",
                table: "ProductsForAppointments");

            migrationBuilder.DropIndex(
                name: "IX_ProductsForAppointments_UserId",
                table: "ProductsForAppointments");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProductsForAppointments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ProductsForAppointments");
        }
    }
}
