using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IvaETicaret.Migrations
{
    public partial class mig_7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adres",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "PostaKodu",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "Sehir",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "Semt",
                table: "OrderHeaders");

            migrationBuilder.AddColumn<int>(
                name: "AdressId",
                table: "OrderHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_AdressId",
                table: "OrderHeaders",
                column: "AdressId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeaders_Adress_AdressId",
                table: "OrderHeaders",
                column: "AdressId",
                principalTable: "Adress",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeaders_Adress_AdressId",
                table: "OrderHeaders");

            migrationBuilder.DropIndex(
                name: "IX_OrderHeaders_AdressId",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "AdressId",
                table: "OrderHeaders");

            migrationBuilder.AddColumn<string>(
                name: "Adres",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostaKodu",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sehir",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Semt",
                table: "OrderHeaders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
