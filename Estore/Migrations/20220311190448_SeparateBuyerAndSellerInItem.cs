using Microsoft.EntityFrameworkCore.Migrations;

namespace Estore.Migrations
{
    public partial class SeparateBuyerAndSellerInItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "Items",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "Items");
        }
    }
}
