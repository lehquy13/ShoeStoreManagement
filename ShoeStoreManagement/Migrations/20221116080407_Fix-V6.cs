using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoeStoreManagement.Migrations
{
    public partial class FixV6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Vouchers_OrderVoucherId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderVoucherId",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "OrderVoucherId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OrderVoucherId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderVoucherId",
                table: "Orders",
                column: "OrderVoucherId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Vouchers_OrderVoucherId",
                table: "Orders",
                column: "OrderVoucherId",
                principalTable: "Vouchers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
