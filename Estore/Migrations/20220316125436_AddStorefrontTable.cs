using Microsoft.EntityFrameworkCore.Migrations;

namespace Estore.Migrations
{
    public partial class AddStorefrontTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StorefrontId",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Storefronts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SummaryText = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storefronts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Storefronts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_StorefrontId",
                table: "Items",
                column: "StorefrontId");

            migrationBuilder.CreateIndex(
                name: "IX_Storefronts_UserId",
                table: "Storefronts",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Storefronts_StorefrontId",
                table: "Items",
                column: "StorefrontId",
                principalTable: "Storefronts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Storefronts_StorefrontId",
                table: "Items");

            migrationBuilder.DropTable(
                name: "Storefronts");

            migrationBuilder.DropIndex(
                name: "IX_Items_StorefrontId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "StorefrontId",
                table: "Items");
        }
    }
}
