using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace ZooStores.Web.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ContactInfo = table.Column<string>(type: "text", nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DeliveryTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    DeliveryCost = table.Column<decimal>(type: "numeric", nullable: false),
                    Area = table.Column<Polygon>(type: "geometry", nullable: false),
                    PaymentTypes = table.Column<List<string>>(type: "text[]", nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExtAttributesTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AttributeName = table.Column<string>(type: "text", nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtAttributesTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocationEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    Region = table.Column<string>(type: "text", nullable: true),
                    Country = table.Column<string>(type: "text", nullable: true),
                    Latitude = table.Column<double>(type: "double precision", nullable: true),
                    Longitude = table.Column<double>(type: "double precision", nullable: true),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MediaFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FileLocalURL = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PetKinds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PetKindName = table.Column<string>(type: "text", nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetKinds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SelfPickupOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PickupPointLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    AvaibableInPlace = table.Column<long>(type: "bigint", nullable: true),
                    DeliveryToPointTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    DeliveryToPointCost = table.Column<TimeSpan>(type: "interval", nullable: false),
                    PaymentTypes = table.Column<string[]>(type: "text[]", nullable: false),
                    IsAvaibableToBook = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelfPickupOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelfPickupOptions_LocationEntity_PickupPointLocationId",
                        column: x => x.PickupPointLocationId,
                        principalTable: "LocationEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VetClinics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Info = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    ownerCompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    locationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VetClinics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VetClinics_Companies_ownerCompanyId",
                        column: x => x.ownerCompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VetClinics_LocationEntity_locationId",
                        column: x => x.locationId,
                        principalTable: "LocationEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VetStores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Info = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    ownerCompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VetStores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VetStores_Companies_ownerCompanyId",
                        column: x => x.ownerCompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VetStores_LocationEntity_LocationId",
                        column: x => x.LocationId,
                        principalTable: "LocationEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Information = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductTypes_ProductCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Information = table.Column<string>(type: "text", nullable: false),
                    ProductTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ManufactureAttributes_Brand = table.Column<string>(type: "text", nullable: false),
                    ManufactureAttributes_CreatorCountry = table.Column<string>(type: "text", nullable: true),
                    ManufactureAttributes_UPC_Code = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    ManufactureAttributes_EAN_Code = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    PhysicalAttributes_Dimensions_Length = table.Column<decimal>(type: "numeric", nullable: true),
                    PhysicalAttributes_Dimensions_Width = table.Column<decimal>(type: "numeric", nullable: true),
                    PhysicalAttributes_Dimensions_Heigth = table.Column<decimal>(type: "numeric", nullable: true),
                    PhysicalAttributes_Materials = table.Column<List<string>>(type: "text[]", nullable: true),
                    PhysicalAttributes_WeightOfProducts = table.Column<decimal>(type: "numeric", nullable: true),
                    PhysicalAttributes_Color = table.Column<string>(type: "text", nullable: true),
                    PhysicalAttributes_ContentMeasurementsUnits = table.Column<int>(type: "integer", nullable: false),
                    PetInfoAttributes_PetAgeWeeksMin = table.Column<long>(type: "bigint", nullable: true),
                    PetInfoAttributes_PetAgeWeeksMax = table.Column<long>(type: "bigint", nullable: true),
                    PetInfoAttributes_PetSize = table.Column<int>(type: "integer", nullable: true),
                    PetInfoAttributes_PetGender = table.Column<int>(type: "integer", nullable: true),
                    PetInfoAttributes_PetKindId = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaURI = table.Column<List<string>>(type: "text[]", nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_PetKinds_PetInfoAttributes_PetKindId",
                        column: x => x.PetInfoAttributes_PetKindId,
                        principalTable: "PetKinds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_ProductTypes_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExtAttributes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AttributeTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    AttributeValue = table.Column<string>(type: "text", nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtAttributes_ExtAttributesTypes_AttributeTypeId",
                        column: x => x.AttributeTypeId,
                        principalTable: "ExtAttributesTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExtAttributes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReviewerName = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<decimal>(type: "numeric", maxLength: 10, nullable: false),
                    Title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    Text = table.Column<string>(type: "text", nullable: false),
                    ProductEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_Products_ProductEntityId",
                        column: x => x.ProductEntityId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SellingSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    DefaultSlotPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Discount = table.Column<decimal>(type: "numeric", nullable: false),
                    IsAvaibable = table.Column<bool>(type: "boolean", nullable: false),
                    VetStoreEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellingSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellingSlots_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellingSlots_VetStores_VetStoreEntityId",
                        column: x => x.VetStoreEntityId,
                        principalTable: "VetStores",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TagName = table.Column<string>(type: "character varying(24)", maxLength: 24, nullable: false),
                    ProductEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Products_ProductEntityId",
                        column: x => x.ProductEntityId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DeliveryOptionEntityProductSlotEntity",
                columns: table => new
                {
                    DeliveryOptionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductSlotsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryOptionEntityProductSlotEntity", x => new { x.DeliveryOptionsId, x.ProductSlotsId });
                    table.ForeignKey(
                        name: "FK_DeliveryOptionEntityProductSlotEntity_DeliveryOptions_Deliv~",
                        column: x => x.DeliveryOptionsId,
                        principalTable: "DeliveryOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryOptionEntityProductSlotEntity_SellingSlots_ProductS~",
                        column: x => x.ProductSlotsId,
                        principalTable: "SellingSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSlotEntitySelfPickupOptionEntity",
                columns: table => new
                {
                    ProductSlotsId = table.Column<Guid>(type: "uuid", nullable: false),
                    SelfPickupOptionsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSlotEntitySelfPickupOptionEntity", x => new { x.ProductSlotsId, x.SelfPickupOptionsId });
                    table.ForeignKey(
                        name: "FK_ProductSlotEntitySelfPickupOptionEntity_SelfPickupOptions_S~",
                        column: x => x.SelfPickupOptionsId,
                        principalTable: "SelfPickupOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSlotEntitySelfPickupOptionEntity_SellingSlots_Produc~",
                        column: x => x.ProductSlotsId,
                        principalTable: "SellingSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOptionEntityProductSlotEntity_ProductSlotsId",
                table: "DeliveryOptionEntityProductSlotEntity",
                column: "ProductSlotsId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtAttributes_AttributeTypeId",
                table: "ExtAttributes",
                column: "AttributeTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtAttributes_ProductId",
                table: "ExtAttributes",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ExtAttributesTypes_AttributeName",
                table: "ExtAttributesTypes",
                column: "AttributeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_FileLocalURL",
                table: "MediaFiles",
                column: "FileLocalURL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PetKinds_PetKindName",
                table: "PetKinds",
                column: "PetKindName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_CategoryName",
                table: "ProductCategories",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_PetInfoAttributes_PetKindId",
                table: "Products",
                column: "PetInfoAttributes_PetKindId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductTypeId",
                table: "Products",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSlotEntitySelfPickupOptionEntity_SelfPickupOptionsId",
                table: "ProductSlotEntitySelfPickupOptionEntity",
                column: "SelfPickupOptionsId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypes_CategoryId",
                table: "ProductTypes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypes_Name",
                table: "ProductTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductEntityId",
                table: "Reviews",
                column: "ProductEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_SelfPickupOptions_PickupPointLocationId",
                table: "SelfPickupOptions",
                column: "PickupPointLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingSlots_ProductId",
                table: "SellingSlots",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingSlots_VetStoreEntityId",
                table: "SellingSlots",
                column: "VetStoreEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_ProductEntityId",
                table: "Tags",
                column: "ProductEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_TagName",
                table: "Tags",
                column: "TagName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VetClinics_locationId",
                table: "VetClinics",
                column: "locationId");

            migrationBuilder.CreateIndex(
                name: "IX_VetClinics_ownerCompanyId",
                table: "VetClinics",
                column: "ownerCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_VetStores_LocationId",
                table: "VetStores",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_VetStores_ownerCompanyId",
                table: "VetStores",
                column: "ownerCompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryOptionEntityProductSlotEntity");

            migrationBuilder.DropTable(
                name: "ExtAttributes");

            migrationBuilder.DropTable(
                name: "MediaFiles");

            migrationBuilder.DropTable(
                name: "ProductSlotEntitySelfPickupOptionEntity");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "VetClinics");

            migrationBuilder.DropTable(
                name: "DeliveryOptions");

            migrationBuilder.DropTable(
                name: "ExtAttributesTypes");

            migrationBuilder.DropTable(
                name: "SelfPickupOptions");

            migrationBuilder.DropTable(
                name: "SellingSlots");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "VetStores");

            migrationBuilder.DropTable(
                name: "PetKinds");

            migrationBuilder.DropTable(
                name: "ProductTypes");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "LocationEntity");

            migrationBuilder.DropTable(
                name: "ProductCategories");
        }
    }
}
