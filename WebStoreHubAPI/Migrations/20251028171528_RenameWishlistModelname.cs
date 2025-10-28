using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebStoreHubAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameWishlistModelname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CartItemId",
                table: "Wishlist",
                newName: "WishlistItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WishlistItemId",
                table: "Wishlist",
                newName: "CartItemId");
        }
    }
}
