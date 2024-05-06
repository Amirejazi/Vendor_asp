using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vendor.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class EditProductProductCategory_Mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSelectedCategories",
                table: "ProductSelectedCategories");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "ProductSelectedCategories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "ProductSelectedCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "ProductSelectedCategories",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "ProductSelectedCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSelectedCategories",
                table: "ProductSelectedCategories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSelectedCategories_ProductId",
                table: "ProductSelectedCategories",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductSelectedCategories",
                table: "ProductSelectedCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductSelectedCategories_ProductId",
                table: "ProductSelectedCategories");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductSelectedCategories");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "ProductSelectedCategories");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "ProductSelectedCategories");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "ProductSelectedCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductSelectedCategories",
                table: "ProductSelectedCategories",
                columns: new[] { "ProductId", "ProductCategoryId" });
        }
    }
}
