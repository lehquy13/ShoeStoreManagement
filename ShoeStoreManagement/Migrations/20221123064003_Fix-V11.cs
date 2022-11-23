using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShoeStoreManagement.Migrations
{
    public partial class FixV11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AvatarName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarName",
                table: "AspNetUsers");
        }
    }
}
