using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zoobee.Web.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductsLineups_ProductLineUpId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ZooStores_NormalizedName",
                table: "ZooStores");

            migrationBuilder.RenameColumn(
                name: "ProductLineUpId",
                table: "Products",
                newName: "ProductLineupId");

            migrationBuilder.RenameColumn(
                name: "Rating",
                table: "Products",
                newName: "AverageRating");

            migrationBuilder.RenameColumn(
                name: "Article",
                table: "Products",
                newName: "SiteArticles");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ProductLineUpId",
                table: "Products",
                newName: "IX_Products_ProductLineupId");

            migrationBuilder.RenameColumn(
                name: "NormalizedCountryName",
                table: "CreatorCountries",
                newName: "NormalizedCountryNameRus");

            migrationBuilder.RenameColumn(
                name: "CountryName",
                table: "CreatorCountries",
                newName: "CountryNameRus");

            migrationBuilder.AddColumn<float>(
                name: "Rating",
                table: "SellingSlots",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "SellingUrl",
                table: "SellingSlots",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UPC",
                table: "Products",
                type: "character varying(12)",
                maxLength: 12,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(12)",
                oldMaxLength: 12);

            migrationBuilder.AlterColumn<string>(
                name: "EAN",
                table: "Products",
                type: "character varying(13)",
                maxLength: 13,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(13)",
                oldMaxLength: 13);

            migrationBuilder.AddColumn<string>(
                name: "NormalizedAreaName",
                table: "DeliveryAreas",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CountryNameEng",
                table: "CreatorCountries",
                type: "character varying(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedCountryNameEng",
                table: "CreatorCountries",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductsLineups_ProductLineupId",
                table: "Products",
                column: "ProductLineupId",
                principalTable: "ProductsLineups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductsLineups_ProductLineupId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "SellingSlots");

            migrationBuilder.DropColumn(
                name: "SellingUrl",
                table: "SellingSlots");

            migrationBuilder.DropColumn(
                name: "NormalizedAreaName",
                table: "DeliveryAreas");

            migrationBuilder.DropColumn(
                name: "CountryNameEng",
                table: "CreatorCountries");

            migrationBuilder.DropColumn(
                name: "NormalizedCountryNameEng",
                table: "CreatorCountries");

            migrationBuilder.RenameColumn(
                name: "ProductLineupId",
                table: "Products",
                newName: "ProductLineUpId");

            migrationBuilder.RenameColumn(
                name: "SiteArticles",
                table: "Products",
                newName: "Article");

            migrationBuilder.RenameColumn(
                name: "AverageRating",
                table: "Products",
                newName: "Rating");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ProductLineupId",
                table: "Products",
                newName: "IX_Products_ProductLineUpId");

            migrationBuilder.RenameColumn(
                name: "NormalizedCountryNameRus",
                table: "CreatorCountries",
                newName: "NormalizedCountryName");

            migrationBuilder.RenameColumn(
                name: "CountryNameRus",
                table: "CreatorCountries",
                newName: "CountryName");

            migrationBuilder.AlterColumn<string>(
                name: "UPC",
                table: "Products",
                type: "character varying(12)",
                maxLength: 12,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(12)",
                oldMaxLength: 12,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EAN",
                table: "Products",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(13)",
                oldMaxLength: 13,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ZooStores_NormalizedName",
                table: "ZooStores",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductsLineups_ProductLineUpId",
                table: "Products",
                column: "ProductLineUpId",
                principalTable: "ProductsLineups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
