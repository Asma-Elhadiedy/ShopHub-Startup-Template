using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace myshop.DAL.Migrations
{
    /// <inheritdoc />
    public partial class removeUniqueindex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_ProductId_ApplicationUserId",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId_ApplicationUserId",
                table: "Reviews",
                columns: new[] { "ProductId", "ApplicationUserId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_ProductId_ApplicationUserId",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId_ApplicationUserId",
                table: "Reviews",
                columns: new[] { "ProductId", "ApplicationUserId" },
                unique: true);
        }
    }
}
