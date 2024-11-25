using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExtractSessionEnumData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // 1. Update SourceType
            migrationBuilder.Sql(@"
            UPDATE ExtractSessions 
            SET SourceType = CASE SourceType
                WHEN '0' THEN 'NutritionFact'
                WHEN '1' THEN 'FirstAttribute'
                WHEN '2' THEN 'SecondAttribute'
                ELSE SourceType
            END;
            ");

            // 2. Update Status
            migrationBuilder.Sql(@"
            UPDATE ExtractSessions 
            SET Status = CASE Status
                WHEN '0' THEN 'Pending'
                WHEN '1' THEN 'Processing'
                WHEN '2' THEN 'Completed'
                WHEN '3' THEN 'Failed'
                ELSE Status
            END;
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            UPDATE ExtractSessions 
            SET SourceType = CASE SourceType
                WHEN 'NutritionFact' THEN '0'
                WHEN 'FirstAttribute' THEN '1'
                WHEN 'SecondAttribute' THEN '2'
                ELSE SourceType
            END;
            ");

            migrationBuilder.Sql(@"
            UPDATE ExtractSessions 
            SET Status = CASE Status
                WHEN 'Pending' THEN '0'
                WHEN 'Processing' THEN '1'
                WHEN 'Completed' THEN '2'
                WHEN 'Failed' THEN '3'
                ELSE Status
            END;
            ");

        }
    }
}
