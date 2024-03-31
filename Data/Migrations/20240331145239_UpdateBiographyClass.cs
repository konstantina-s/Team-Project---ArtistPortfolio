using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtistPortfolio.Data.Migrations
{
    public partial class UpdateBiographyClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageData",
                table: "Biography",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Biography");
        }
    }
}
