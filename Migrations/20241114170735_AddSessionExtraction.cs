using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApi.Migrations
{
    /// <inheritdoc />
    public partial class AddSessionExtraction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompareResult",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ValidationResult",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "ExtractSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ExtractedData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RawExtractData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourceType = table.Column<int>(type: "int", nullable: false),
                    ExtractorVersion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtractSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExtractSessions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExtractSessions_ProductId_SourceType_CreatedAt",
                table: "ExtractSessions",
                columns: new[] { "ProductId", "SourceType", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExtractSessions");

            migrationBuilder.AddColumn<string>(
                name: "CompareResult",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ValidationResult",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
