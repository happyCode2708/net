using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueIdAndAutoIncrementId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // Step 1: Drop foreign key from product image many-to-many table
            migrationBuilder.DropForeignKey(
                name: "FK_ProductImage_Products_ProductId",
                table: "ProductImage");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductImage_Image_ImageId",
                table: "ProductImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductImage",
                table: "ProductImage"
            );
            // Step 2: Drop the primary key on Products and Image
            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
name: "PK_Image",
table: "Image");

            //? Step 1: Add UniqueId columns to Products and Image tables
            migrationBuilder.AddColumn<string>(
                name: "UniqueId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UniqueId",
                table: "Image",
                type: "nvarchar(450)",
                nullable: true,
                defaultValue: "");

            //? Step 2: Copy UUIDs from Id columns to UniqueId columns
            migrationBuilder.Sql("UPDATE Products SET UniqueId = Id");
            migrationBuilder.Sql("UPDATE Image SET UniqueId = Id");

            //? Step 3: Add new auto-increment integer Id columns (NewId) to Products and Image tables
            migrationBuilder.AddColumn<int>(
                name: "NewId",
                table: "Products",
                nullable: false,
                defaultValue: 0
            ).Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "NewId",
                table: "Image",
                nullable: false,
                defaultValue: 0
            ).Annotation("SqlServer:Identity", "1, 1");

            //? Step 4: Drop old string-based Id columns
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Image");

            //? Step 5: Rename NewId columns to Id
            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "Products",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "Image",
                newName: "Id");

            //? Step 6: Add temporary columns to store new integer IDs in ProductImage table
            migrationBuilder.AddColumn<int>(
                name: "NewProductId",
                table: "ProductImage",
                nullable: false, defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NewImageId",
                table: "ProductImage",
                nullable: false,
                defaultValue: 0);

            //? Step 7: Update ProductImage with new integer IDs based on UniqueId matching
            migrationBuilder.Sql(@"
        UPDATE pi
        SET NewProductId = p.Id
        FROM ProductImage pi
        INNER JOIN Products p ON pi.ProductId = p.UniqueId
    ");

            migrationBuilder.Sql(@"
        UPDATE pi
        SET NewImageId = i.Id
        FROM ProductImage pi
        INNER JOIN Image i ON pi.ImageId = i.UniqueId
    ");

            // //? need to drop index productImage table before we can drop column productId and imageId
            migrationBuilder.DropIndex(name: "IX_ProductImage_ImageId", table: "ProductImage");
            // migrationBuilder.DropIndex(name: "IX_ProductImage_ProductId", table: "ProductImage");

            //? Step 8: Drop old ProductId and ImageId columns from ProductImage
            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductImage");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "ProductImage");

            //? Step 9: Rename NewProductId and NewImageId to ProductId and ImageId
            migrationBuilder.RenameColumn(
                name: "NewProductId",
                table: "ProductImage",
                newName: "ProductId");

            migrationBuilder.RenameColumn(
                name: "NewImageId",
                table: "ProductImage",
                newName: "ImageId");

            //? Step 10: Ensure UniqueId columns are not nullable and are unique
            migrationBuilder.AlterColumn<string>(
                name: "UniqueId",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "UniqueId",
                table: "Image",
                type: "nvarchar(450)",
                nullable: false);

            //? add primary key to tables

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductImage",
                table: "ProductImage",
                columns: new[] { "ProductId", "ImageId" }
            );

            migrationBuilder.AddPrimaryKey(
       name: "PK_Products",
       table: "Products",
       column: "Id"
   );

            migrationBuilder.AddPrimaryKey(
    name: "PK_Image",
    table: "Image",
    column: "Id"

);

            //? add foreign key to many-to-many table

            migrationBuilder.AddForeignKey(
    name: "FK_ProductImage_Products_ProductId",
    table: "ProductImage",
    column: "ProductId",
    principalTable: "Products",
    principalColumn: "Id",
    onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
name: "FK_ProductImage_Image_ImageId",
table: "ProductImage",
column: "ImageId",
principalTable: "Image",
principalColumn: "Id",
onDelete: ReferentialAction.Cascade);

            //? create index for image and product table

            migrationBuilder.CreateIndex(
                name: "IX_Products_UniqueId",
                table: "Products",
                column: "UniqueId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Image_UniqueId",
                table: "Image",
                column: "UniqueId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            //? Step 1: Drop unique indexes on UniqueId columns
            migrationBuilder.DropIndex(name: "IX_Products_UniqueId", table: "Products");
            migrationBuilder.DropIndex(name: "IX_Image_UniqueId", table: "Image");

            //? Step 2: Drop the UniqueId columns
            migrationBuilder.DropColumn(name: "UniqueId", table: "Products");
            migrationBuilder.DropColumn(name: "UniqueId", table: "Image");

            //? Step 3: Add old string-based ProductId and ImageId columns back to ProductImage
            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "ProductImage",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "ProductImage",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            //? Step 4: Copy data from new integer ProductId and ImageId to old string columns
            migrationBuilder.Sql(@"
        UPDATE pi
        SET pi.ProductId = p.UniqueId
        FROM ProductImage pi
        INNER JOIN Products p ON pi.ProductId = p.Id
    ");

            migrationBuilder.Sql(@"
        UPDATE pi
        SET pi.ImageId = i.UniqueId
        FROM ProductImage pi
        INNER JOIN Image i ON pi.ImageId = i.Id
    ");

            //? Step 5: Drop new integer ProductId and ImageId columns from ProductImage
            migrationBuilder.DropColumn(name: "ProductId", table: "ProductImage");
            migrationBuilder.DropColumn(name: "ImageId", table: "ProductImage");

            //? Step 6: Rename integer Id columns back to string-based Id columns
            migrationBuilder.RenameColumn(name: "Id", table: "Products", newName: "OldId");
            migrationBuilder.RenameColumn(name: "Id", table: "Image", newName: "OldId");

            //? Step 7: Add back old string-based Id columns
            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Image",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            //? Step 8: Copy data from UniqueId columns back to old Id columns
            migrationBuilder.Sql("UPDATE Products SET Id = UniqueId");
            migrationBuilder.Sql("UPDATE Image SET Id = UniqueId");

            //? Step 9: Drop the NewId columns
            migrationBuilder.DropColumn(name: "OldId", table: "Products");
            migrationBuilder.DropColumn(name: "OldId", table: "Image");
        }
    }
}
