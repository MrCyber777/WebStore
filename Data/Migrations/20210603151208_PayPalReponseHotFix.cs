using Microsoft.EntityFrameworkCore.Migrations;

namespace WebStore.Data.Migrations
{
    public partial class PayPalReponseHotFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "PayPalResponses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PayPalResponses_AppointmentId",
                table: "PayPalResponses",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PayPalResponses_Appointments_AppointmentId",
                table: "PayPalResponses",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PayPalResponses_Appointments_AppointmentId",
                table: "PayPalResponses");

            migrationBuilder.DropIndex(
                name: "IX_PayPalResponses_AppointmentId",
                table: "PayPalResponses");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "PayPalResponses");
        }
    }
}
