using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vendor.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class EditProductGaller_Mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductGalleries_Products_ProductId1",
                table: "ProductGalleries");

            migrationBuilder.DropIndex(
                name: "IX_ProductGalleries_ProductId1",
                table: "ProductGalleries");

            migrationBuilder.DropColumn(
                name: "ProductId1",
                table: "ProductGalleries");

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "ProductGalleries",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGalleries_ProductId",
                table: "ProductGalleries",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGalleries_Products_ProductId",
                table: "ProductGalleries",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductGalleries_Products_ProductId",
                table: "ProductGalleries");

            migrationBuilder.DropIndex(
                name: "IX_ProductGalleries_ProductId",
                table: "ProductGalleries");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductGalleries",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "ProductId1",
                table: "ProductGalleries",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ProductGalleries_ProductId1",
                table: "ProductGalleries",
                column: "ProductId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGalleries_Products_ProductId1",
                table: "ProductGalleries",
                column: "ProductId1",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
