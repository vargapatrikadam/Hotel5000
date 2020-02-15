using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Lodgings.Migrations
{
    public partial class add_lodging_type : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "LodgingType",
                newName: "LodgingTypes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "LodgingTypes",
                newName: "LodgingType");
        }
    }
}
