using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImageWithMetaAndRemovePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Images",
                newName: "OriginalFileName");

            migrationBuilder.RenameColumn(
                name: "StoredFileName",
                table: "Images",
                newName: "MimeType");

            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Images",
                newName: "ImageSize");

            migrationBuilder.RenameColumn(
                name: "OriginFileName",
                table: "Images",
                newName: "ImageName");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Images",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Extension",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "Images",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsRaw",
                table: "Images",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Extension",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "IsRaw",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "OriginalFileName",
                table: "Images",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "MimeType",
                table: "Images",
                newName: "StoredFileName");

            migrationBuilder.RenameColumn(
                name: "ImageSize",
                table: "Images",
                newName: "Path");

            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "Images",
                newName: "OriginFileName");
        }
    }
}
