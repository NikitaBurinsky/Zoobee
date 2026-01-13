using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zoobee.Web.Migrations.ParsersDb
{
    /// <inheritdoc />
    public partial class parse_init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScrapingTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    SourceName = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    NextTryAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AttemptCount = table.Column<int>(type: "integer", nullable: false),
                    CustomFrequencyHours = table.Column<int>(type: "integer", nullable: true),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrapingTasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScrapingDatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ScrapingTaskId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    HttpStatusCode = table.Column<int>(type: "integer", nullable: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true),
                    DeleteData_DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeleteData_IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Metadata_CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Metadata_LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScrapingDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScrapingDatas_ScrapingTasks_ScrapingTaskId",
                        column: x => x.ScrapingTaskId,
                        principalTable: "ScrapingTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScrapingDatas_ScrapingTaskId",
                table: "ScrapingDatas",
                column: "ScrapingTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_ScrapingTasks_Status_NextTryAt",
                table: "ScrapingTasks",
                columns: new[] { "Status", "NextTryAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ScrapingTasks_Url",
                table: "ScrapingTasks",
                column: "Url",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScrapingDatas");

            migrationBuilder.DropTable(
                name: "ScrapingTasks");
        }
    }
}
