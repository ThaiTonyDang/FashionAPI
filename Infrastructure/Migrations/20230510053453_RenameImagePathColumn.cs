using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class RenameImagePathColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Products",
                newName: "ImageName");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Categories",
                newName: "ImageName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "Products",
                newName: "ImagePath");

            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "Categories",
                newName: "ImagePath");
        }
    }
}
