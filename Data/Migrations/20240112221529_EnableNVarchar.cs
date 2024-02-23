using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtistPortfolio.Data.Migrations
{
    public partial class EnableNVarchar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TitleMK",
                table: "Images",
                type: "nvarchar(200)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(200)");

            migrationBuilder.AlterColumn<string>(
                name: "TechniqueMK",
                table: "Images",
                type: "nvarchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TitleMK",
                table: "Images",
                type: "varchar(200)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)");

            migrationBuilder.AlterColumn<string>(
                name: "TechniqueMK",
                table: "Images",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");
        }
    }
}
