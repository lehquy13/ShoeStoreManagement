using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoeStoreManagement.Migrations
{
    public partial class FixV7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CartTotalAmount",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CartTotalAmountSelected",
                table: "Carts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CartTotalAmount",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "CartTotalAmountSelected",
                table: "Carts");
        }
    }
}
