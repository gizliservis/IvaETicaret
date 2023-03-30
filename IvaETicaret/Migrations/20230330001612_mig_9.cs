using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IvaETicaret.Migrations
{
    public partial class mig_9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdressTitle",
                table: "Adress",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdressTitle",
                table: "Adress");
        }
    }
}
