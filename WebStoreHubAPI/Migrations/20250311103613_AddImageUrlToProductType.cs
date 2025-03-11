using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebStoreHubAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUrlToProductType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "DbProductTypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "DbProductTypes");
        }
    }
}
