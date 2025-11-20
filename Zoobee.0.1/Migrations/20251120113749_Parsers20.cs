using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zoobee.Web.Migrations
{
    /// <inheritdoc />
    public partial class Parsers20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ParsedProductsIdentifiers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ParsedName = table.Column<string>(type: "text", nullable: false),
                    ParsedUrl = table.Column<string>(type: "text", nullable: false),
                    ParsedDatasOfProduct = table.Column<List<Guid>>(type: "uuid[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParsedProductsIdentifiers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ZoobazarParsedProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HaveOffers = table.Column<bool>(type: "boolean", nullable: true),
                    CurrentLocation_Id = table.Column<int>(type: "integer", nullable: true),
                    CurrentLocation_Code = table.Column<string>(type: "text", nullable: true),
                    CurrentLocation_Name = table.Column<string>(type: "text", nullable: true),
                    CurrentLocation_Store = table.Column<List<int>>(type: "integer[]", nullable: true),
                    CurrentLocation_Location = table.Column<List<string>>(type: "text[]", nullable: true),
                    CurrentLocation_FreeDelivery = table.Column<int>(type: "integer", nullable: true),
                    CurrentLocation_Hours = table.Column<string>(type: "text", nullable: true),
                    CurrentLocation_PickupOnly = table.Column<bool>(type: "boolean", nullable: true),
                    User_Id = table.Column<int>(type: "integer", nullable: true),
                    User_PersonalPhone = table.Column<string>(type: "text", nullable: true),
                    User_Name = table.Column<string>(type: "text", nullable: true),
                    SiteId = table.Column<string>(type: "text", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: true),
                    ProductXmlId = table.Column<string>(type: "text", nullable: true),
                    ProductName = table.Column<string>(type: "text", nullable: true),
                    SectionId = table.Column<string>(type: "text", nullable: true),
                    SectionName = table.Column<string>(type: "text", nullable: true),
                    SectionPath = table.Column<string>(type: "text", nullable: true),
                    Rating_Value = table.Column<double>(type: "double precision", nullable: true),
                    Rating_Count = table.Column<int>(type: "integer", nullable: true),
                    Composition = table.Column<string>(type: "text", nullable: true),
                    BrandId = table.Column<string>(type: "text", nullable: true),
                    Brand_Id = table.Column<string>(type: "text", nullable: true),
                    Brand_Name = table.Column<string>(type: "text", nullable: true),
                    Brand_Link = table.Column<string>(type: "text", nullable: true),
                    Advantages = table.Column<string>(type: "text", nullable: true),
                    OffersProperty = table.Column<string>(type: "text", nullable: true),
                    Test_Brand_Id = table.Column<string>(type: "text", nullable: true),
                    Test_Brand_IblockId = table.Column<string>(type: "text", nullable: true),
                    Test_Brand_Active = table.Column<string>(type: "text", nullable: true),
                    Test_Brand_Name = table.Column<string>(type: "text", nullable: true),
                    Test_Gallery_Id = table.Column<string>(type: "text", nullable: true),
                    Test_Gallery_IblockId = table.Column<string>(type: "text", nullable: true),
                    Test_Gallery_Active = table.Column<string>(type: "text", nullable: true),
                    Test_Gallery_Name = table.Column<string>(type: "text", nullable: true),
                    Reviews_Statistic_Count = table.Column<int>(type: "integer", nullable: true),
                    Reviews_Statistic_Vote = table.Column<double>(type: "double precision", nullable: true),
                    Reviews_Statistic_Allow = table.Column<bool>(type: "boolean", nullable: true),
                    Reviews_Statistic_IsExists = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoobazarParsedProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zoobazar_ParsedProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    parsedProductModelId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LastTransformed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ParsedUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zoobazar_ParsedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zoobazar_ParsedProducts_ZoobazarParsedProducts_parsedProduc~",
                        column: x => x.parsedProductModelId,
                        principalTable: "ZoobazarParsedProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZoobazarOffers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SiteId = table.Column<int>(type: "integer", nullable: true),
                    XmlId = table.Column<string>(type: "text", nullable: true),
                    MindboxId = table.Column<string>(type: "text", nullable: true),
                    Selected = table.Column<bool>(type: "boolean", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Label = table.Column<string>(type: "text", nullable: true),
                    Link = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Sku = table.Column<string>(type: "text", nullable: true),
                    InternalCode = table.Column<string>(type: "text", nullable: true),
                    Available = table.Column<bool>(type: "boolean", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: true),
                    DateDelivery = table.Column<string>(type: "text", nullable: true),
                    Weight = table.Column<string>(type: "text", nullable: true),
                    Price_Id = table.Column<Guid>(type: "uuid", nullable: true),
                    Price_Base = table.Column<string>(type: "text", nullable: true),
                    Price_Result = table.Column<string>(type: "text", nullable: true),
                    Price_DiscountPercent = table.Column<int>(type: "integer", nullable: true),
                    Price_BaseRaw = table.Column<double>(type: "double precision", nullable: true),
                    Price_ResultRaw = table.Column<double>(type: "double precision", nullable: true),
                    Price_Currency = table.Column<string>(type: "text", nullable: true),
                    Price_SubscriptionRaw = table.Column<double>(type: "double precision", nullable: true),
                    Price_Format_Pattern = table.Column<string>(type: "text", nullable: true),
                    Price_Format_DecimalPoint = table.Column<string>(type: "text", nullable: true),
                    Price_Format_ThousandsSeparator = table.Column<string>(type: "text", nullable: true),
                    Price_Format_Decimals = table.Column<int>(type: "integer", nullable: true),
                    DiscountPoints_Raw = table.Column<double>(type: "double precision", nullable: true),
                    DiscountPoints_Formatted = table.Column<string>(type: "text", nullable: true),
                    Sale = table.Column<string>(type: "jsonb", nullable: false),
                    IsMedicine = table.Column<bool>(type: "boolean", nullable: true),
                    IsAvailDelivery = table.Column<bool>(type: "boolean", nullable: true),
                    ZoobazarParsedProductId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoobazarOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZoobazarOffers_ZoobazarParsedProducts_ZoobazarParsedProduct~",
                        column: x => x.ZoobazarParsedProductId,
                        principalTable: "ZoobazarParsedProducts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ZoobazarTabs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "text", nullable: true),
                    Sort = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true),
                    ZoobazarParsedProductId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoobazarTabs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZoobazarTabs_ZoobazarParsedProducts_ZoobazarParsedProductId",
                        column: x => x.ZoobazarParsedProductId,
                        principalTable: "ZoobazarParsedProducts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Property",
                columns: table => new
                {
                    PropertyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Value = table.Column<string>(type: "text", nullable: true),
                    OfferId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Property", x => x.PropertyId);
                    table.ForeignKey(
                        name: "FK_Property_ZoobazarOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "ZoobazarOffers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    TagId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<string>(type: "jsonb", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Link = table.Column<string>(type: "text", nullable: true),
                    OfferId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.TagId);
                    table.ForeignKey(
                        name: "FK_Tag_ZoobazarOffers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "ZoobazarOffers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Property_OfferId",
                table: "Property",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_OfferId",
                table: "Tag",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Zoobazar_ParsedProducts_parsedProductModelId",
                table: "Zoobazar_ParsedProducts",
                column: "parsedProductModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoobazarOffers_ZoobazarParsedProductId",
                table: "ZoobazarOffers",
                column: "ZoobazarParsedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoobazarTabs_ZoobazarParsedProductId",
                table: "ZoobazarTabs",
                column: "ZoobazarParsedProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ParsedProductsIdentifiers");

            migrationBuilder.DropTable(
                name: "Property");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "Zoobazar_ParsedProducts");

            migrationBuilder.DropTable(
                name: "ZoobazarTabs");

            migrationBuilder.DropTable(
                name: "ZoobazarOffers");

            migrationBuilder.DropTable(
                name: "ZoobazarParsedProducts");
        }
    }
}
