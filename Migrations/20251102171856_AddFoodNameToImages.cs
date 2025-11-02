using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealCheckAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddFoodNameToImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FoodName",
                table: "Images",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FoodName",
                table: "Images");
        }
    }
}
