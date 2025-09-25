using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ZooStores.Web.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserType = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    BornYear = table.Column<int>(type: "integer", nullable: true),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BrandName = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    NormalizedBrandName = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    Description = table.Column<string>(type: "character varying(750)", maxLength: 750, nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreatorCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    NormalizedCompanyName = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatorCompanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreatorCountries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CountryName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    NormalizedCountryName = table.Column<string>(type: "text", nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatorCountries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    NormalizedAddress = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    NormalizedCity = table.Column<string>(type: "text", nullable: true),
                    GeoPoint_Longitude = table.Column<double>(type: "double precision", nullable: false),
                    GeoPoint_Latitude = table.Column<double>(type: "double precision", nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
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
                    PetKindName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    NormalizedPetKindName = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
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
                name: "SellerCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    NormalizedCompanyName = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerCompanies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductsLineups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BrandId = table.Column<Guid>(type: "uuid", nullable: false),
                    LineupName = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    NormalizedLineupName = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    LineupDescription = table.Column<string>(type: "character varying(750)", maxLength: 750, nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsLineups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsLineups_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryAreas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AreaName = table.Column<string>(type: "text", nullable: false),
                    IsTemplate = table.Column<bool>(type: "boolean", nullable: false),
                    SellerCompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAreas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryAreas_SellerCompanies_SellerCompanyId",
                        column: x => x.SellerCompanyId,
                        principalTable: "SellerCompanies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ZooStores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StoreType = table.Column<int>(type: "integer", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    SellerCompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    NormalizedName = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false),
                    OpeningTime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    ClosingTime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZooStores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZooStores_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ZooStores_SellerCompanies_SellerCompanyId",
                        column: x => x.SellerCompanyId,
                        principalTable: "SellerCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    NormalizedName = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
                    Article = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    UPC = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    EAN = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    Rating = table.Column<float>(type: "real", nullable: false),
                    MinPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    MaxPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatorCountryId = table.Column<Guid>(type: "uuid", nullable: false),
                    BrandId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductLineUpId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatorCompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    PetKindId = table.Column<Guid>(type: "uuid", nullable: false),
                    MediaURI = table.Column<List<string>>(type: "text[]", nullable: false),
                    Discriminator = table.Column<string>(type: "character varying(21)", maxLength: 21, nullable: false),
                    FoodType = table.Column<int>(type: "integer", nullable: true),
                    PetAgeRange_PetAgeWeeksMin = table.Column<long>(type: "bigint", nullable: true),
                    PetAgeRange_PetAgeWeeksMax = table.Column<long>(type: "bigint", nullable: true),
                    ProductWeightGramms = table.Column<decimal>(type: "numeric", nullable: true),
                    ToiletType = table.Column<int>(type: "integer", nullable: true),
                    Dimensions_X = table.Column<float>(type: "real", nullable: true),
                    Dimensions_Y = table.Column<float>(type: "real", nullable: true),
                    Dimensions_Z = table.Column<float>(type: "real", nullable: true),
                    VolumeLiters = table.Column<float>(type: "real", nullable: true),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_CreatorCompanies_CreatorCompanyId",
                        column: x => x.CreatorCompanyId,
                        principalTable: "CreatorCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_CreatorCountries_CreatorCountryId",
                        column: x => x.CreatorCountryId,
                        principalTable: "CreatorCountries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_PetKinds_PetKindId",
                        column: x => x.PetKindId,
                        principalTable: "PetKinds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_ProductsLineups_ProductLineUpId",
                        column: x => x.ProductLineUpId,
                        principalTable: "ProductsLineups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryAreas_GeoArea",
                columns: table => new
                {
                    DeliveryAreaEntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryAreas_GeoArea", x => new { x.DeliveryAreaEntityId, x.Id });
                    table.ForeignKey(
                        name: "FK_DeliveryAreas_GeoArea_DeliveryAreas_DeliveryAreaEntityId",
                        column: x => x.DeliveryAreaEntityId,
                        principalTable: "DeliveryAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DeliveryPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    DeliveryAreaId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryOptions_DeliveryAreas_DeliveryAreaId",
                        column: x => x.DeliveryAreaId,
                        principalTable: "DeliveryAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelfPickupOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PickupPointLocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeliveryToPointTime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    DeliveryToPointCost = table.Column<decimal>(type: "numeric", nullable: false),
                    IsAvaibableToBook = table.Column<bool>(type: "boolean", nullable: false),
                    IsAvaibableOnlinePayment = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelfPickupOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelfPickupOptions_ZooStores_PickupPointLocationId",
                        column: x => x.PickupPointLocationId,
                        principalTable: "ZooStores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReviewerUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Rating = table.Column<float>(type: "real", nullable: false),
                    Text = table.Column<string>(type: "character varying(750)", maxLength: 750, nullable: false),
                    ReviewedProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_ReviewerUserId",
                        column: x => x.ReviewerUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Products_ReviewedProductId",
                        column: x => x.ReviewedProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SellingSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerCompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    DefaultSlotPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Discount = table.Column<decimal>(type: "numeric", nullable: false),
                    ResultPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
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
                        name: "FK_SellingSlots_SellerCompanies_SellerCompanyId",
                        column: x => x.SellerCompanyId,
                        principalTable: "SellerCompanies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TagName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    NormalizedTagName = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    ProductType = table.Column<string>(type: "text", nullable: false),
                    BaseProductEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_Products_BaseProductEntityId",
                        column: x => x.BaseProductEntityId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DeliveryOptionEntitySellingSlotEntity",
                columns: table => new
                {
                    DeliveryOptionsId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductSlotsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryOptionEntitySellingSlotEntity", x => new { x.DeliveryOptionsId, x.ProductSlotsId });
                    table.ForeignKey(
                        name: "FK_DeliveryOptionEntitySellingSlotEntity_DeliveryOptions_Deliv~",
                        column: x => x.DeliveryOptionsId,
                        principalTable: "DeliveryOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeliveryOptionEntitySellingSlotEntity_SellingSlots_ProductS~",
                        column: x => x.ProductSlotsId,
                        principalTable: "SellingSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SelfPickupOptionEntitySellingSlotEntity",
                columns: table => new
                {
                    ProductSlotsId = table.Column<Guid>(type: "uuid", nullable: false),
                    SelfPickupOptionsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelfPickupOptionEntitySellingSlotEntity", x => new { x.ProductSlotsId, x.SelfPickupOptionsId });
                    table.ForeignKey(
                        name: "FK_SelfPickupOptionEntitySellingSlotEntity_SelfPickupOptions_S~",
                        column: x => x.SelfPickupOptionsId,
                        principalTable: "SelfPickupOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SelfPickupOptionEntitySellingSlotEntity_SellingSlots_Produc~",
                        column: x => x.ProductSlotsId,
                        principalTable: "SellingSlots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_NormalizedBrandName",
                table: "Brands",
                column: "NormalizedBrandName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreatorCompanies_NormalizedCompanyName",
                table: "CreatorCompanies",
                column: "NormalizedCompanyName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAreas_AreaName",
                table: "DeliveryAreas",
                column: "AreaName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryAreas_SellerCompanyId",
                table: "DeliveryAreas",
                column: "SellerCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOptionEntitySellingSlotEntity_ProductSlotsId",
                table: "DeliveryOptionEntitySellingSlotEntity",
                column: "ProductSlotsId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryOptions_DeliveryAreaId",
                table: "DeliveryOptions",
                column: "DeliveryAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFiles_FileLocalURL",
                table: "MediaFiles",
                column: "FileLocalURL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PetKinds_NormalizedPetKindName",
                table: "PetKinds",
                column: "NormalizedPetKindName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatorCompanyId",
                table: "Products",
                column: "CreatorCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatorCountryId",
                table: "Products",
                column: "CreatorCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_NormalizedName",
                table: "Products",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_PetKindId",
                table: "Products",
                column: "PetKindId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductLineUpId",
                table: "Products",
                column: "ProductLineUpId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductsLineups_BrandId",
                table: "ProductsLineups",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewedProductId",
                table: "Reviews",
                column: "ReviewedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ReviewerUserId",
                table: "Reviews",
                column: "ReviewerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SelfPickupOptionEntitySellingSlotEntity_SelfPickupOptionsId",
                table: "SelfPickupOptionEntitySellingSlotEntity",
                column: "SelfPickupOptionsId");

            migrationBuilder.CreateIndex(
                name: "IX_SelfPickupOptions_PickupPointLocationId",
                table: "SelfPickupOptions",
                column: "PickupPointLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerCompanies_NormalizedCompanyName",
                table: "SellerCompanies",
                column: "NormalizedCompanyName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellingSlots_ProductId",
                table: "SellingSlots",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SellingSlots_SellerCompanyId",
                table: "SellingSlots",
                column: "SellerCompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_BaseProductEntityId",
                table: "Tags",
                column: "BaseProductEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_NormalizedTagName",
                table: "Tags",
                column: "NormalizedTagName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ZooStores_LocationId",
                table: "ZooStores",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ZooStores_NormalizedName",
                table: "ZooStores",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ZooStores_SellerCompanyId",
                table: "ZooStores",
                column: "SellerCompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "DeliveryAreas_GeoArea");

            migrationBuilder.DropTable(
                name: "DeliveryOptionEntitySellingSlotEntity");

            migrationBuilder.DropTable(
                name: "MediaFiles");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "SelfPickupOptionEntitySellingSlotEntity");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "DeliveryOptions");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SelfPickupOptions");

            migrationBuilder.DropTable(
                name: "SellingSlots");

            migrationBuilder.DropTable(
                name: "DeliveryAreas");

            migrationBuilder.DropTable(
                name: "ZooStores");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "SellerCompanies");

            migrationBuilder.DropTable(
                name: "CreatorCompanies");

            migrationBuilder.DropTable(
                name: "CreatorCountries");

            migrationBuilder.DropTable(
                name: "PetKinds");

            migrationBuilder.DropTable(
                name: "ProductsLineups");

            migrationBuilder.DropTable(
                name: "Brands");
        }
    }
}
