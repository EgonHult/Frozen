using Microsoft.EntityFrameworkCore.Migrations;

namespace Products.Migrations
{
    public partial class Added_Weight_Property : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WeightInGrams",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeightInGrams",
                table: "Product");
        }
    }
}
