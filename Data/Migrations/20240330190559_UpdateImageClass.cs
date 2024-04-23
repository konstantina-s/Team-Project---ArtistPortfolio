using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtistPortfolio.Data.Migrations
{
    public partial class UpdateImageClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Data",
                table: "Images",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Data",
                table: "Images");
        }
    }
}
