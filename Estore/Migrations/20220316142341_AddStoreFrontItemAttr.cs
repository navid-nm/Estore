using Microsoft.EntityFrameworkCore.Migrations;

namespace Estore.Migrations
{
    public partial class AddStoreFrontItemAttr : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "StorefrontItem",
                table: "Items",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorefrontItem",
                table: "Items");
        }
    }
}
