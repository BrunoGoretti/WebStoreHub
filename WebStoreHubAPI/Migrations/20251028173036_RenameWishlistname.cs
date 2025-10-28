using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebStoreHubAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameWishlistname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishlist_DbProducts_ProductId",
                table: "Wishlist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Wishlist",
                table: "Wishlist");

            migrationBuilder.RenameTable(
                name: "Wishlist",
                newName: "DbWishlist");

            migrationBuilder.RenameIndex(
                name: "IX_Wishlist_ProductId",
                table: "DbWishlist",
                newName: "IX_DbWishlist_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DbWishlist",
                table: "DbWishlist",
                column: "WishlistItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_DbWishlist_DbProducts_ProductId",
                table: "DbWishlist",
                column: "ProductId",
                principalTable: "DbProducts",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DbWishlist_DbProducts_ProductId",
                table: "DbWishlist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DbWishlist",
                table: "DbWishlist");

            migrationBuilder.RenameTable(
                name: "DbWishlist",
                newName: "Wishlist");

            migrationBuilder.RenameIndex(
                name: "IX_DbWishlist_ProductId",
                table: "Wishlist",
                newName: "IX_Wishlist_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Wishlist",
                table: "Wishlist",
                column: "WishlistItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_DbProducts_ProductId",
                table: "Wishlist",
                column: "ProductId",
                principalTable: "DbProducts",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
