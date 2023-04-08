using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IvaETicaret.Migrations
{
    public partial class mig_11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OdemeTurId",
                table: "OrderHeaders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Adress",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "OdemeTurleri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdemeTurleri", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderHeaders_OdemeTurId",
                table: "OrderHeaders",
                column: "OdemeTurId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderHeaders_OdemeTurleri_OdemeTurId",
                table: "OrderHeaders",
                column: "OdemeTurId",
                principalTable: "OdemeTurleri",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderHeaders_OdemeTurleri_OdemeTurId",
                table: "OrderHeaders");

            migrationBuilder.DropTable(
                name: "OdemeTurleri");

            migrationBuilder.DropIndex(
                name: "IX_OrderHeaders_OdemeTurId",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "OdemeTurId",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Adress");
        }
    }
}
