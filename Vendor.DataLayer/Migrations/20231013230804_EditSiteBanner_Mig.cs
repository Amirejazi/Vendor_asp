using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vendor.DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class EditSiteBanner_Mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BannerPlacement",
                table: "SiteBanners",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannerPlacement",
                table: "SiteBanners");
        }
    }
}
