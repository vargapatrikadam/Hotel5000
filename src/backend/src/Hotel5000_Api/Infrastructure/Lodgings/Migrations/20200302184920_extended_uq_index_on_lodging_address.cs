using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Lodgings.Migrations
{
    public partial class extended_uq_index_on_lodging_address : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "LodgingAddress_UQ",
                table: "LodgingAddresses");

            migrationBuilder.CreateIndex(
                name: "LodgingAddress_UQ",
                table: "LodgingAddresses",
                columns: new[] { "CountryId", "County", "City", "PostalCode", "Street", "HouseNumber", "Floor", "DoorNumber" },
                unique: true,
                filter: "IsDeleted = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "LodgingAddress_UQ",
                table: "LodgingAddresses");

            migrationBuilder.CreateIndex(
                name: "LodgingAddress_UQ",
                table: "LodgingAddresses",
                columns: new[] { "CountryId", "County", "City", "PostalCode", "Street" },
                unique: true,
                filter: "IsDeleted = 0");
        }
    }
}
