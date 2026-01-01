using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothStore_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_GenderCategorys_GenderCategoryId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GenderCategorys",
                table: "GenderCategorys");

            migrationBuilder.RenameTable(
                name: "GenderCategorys",
                newName: "GenderCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GenderCategories",
                table: "GenderCategories",
                column: "GenderCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_GenderCategories_GenderCategoryId",
                table: "Products",
                column: "GenderCategoryId",
                principalTable: "GenderCategories",
                principalColumn: "GenderCategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_GenderCategories_GenderCategoryId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GenderCategories",
                table: "GenderCategories");

            migrationBuilder.RenameTable(
                name: "GenderCategories",
                newName: "GenderCategorys");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GenderCategorys",
                table: "GenderCategorys",
                column: "GenderCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_GenderCategorys_GenderCategoryId",
                table: "Products",
                column: "GenderCategoryId",
                principalTable: "GenderCategorys",
                principalColumn: "GenderCategoryId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
