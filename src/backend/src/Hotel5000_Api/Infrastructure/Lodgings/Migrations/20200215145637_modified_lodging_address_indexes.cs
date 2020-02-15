using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Lodgings.Migrations
{
    public partial class modified_lodging_address_indexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_LodgingAddresses_CountryId_County_City_PostalCode_Street_HouseNumber_Floor_DoorNumber",
                table: "LodgingAddresses");

            migrationBuilder.AlterColumn<string>(
                name: "Floor",
                table: "LodgingAddresses",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "DoorNumber",
                table: "LodgingAddresses",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.CreateIndex(
                name: "IX_LodgingAddresses_CountryId_County_City_PostalCode_Street",
                table: "LodgingAddresses",
                columns: new[] { "CountryId", "County", "City", "PostalCode", "Street" },
                unique: true,
                filter: "IsDeleted = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LodgingAddresses_CountryId_County_City_PostalCode_Street",
                table: "LodgingAddresses");

            migrationBuilder.AlterColumn<string>(
                name: "Floor",
                table: "LodgingAddresses",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DoorNumber",
                table: "LodgingAddresses",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_LodgingAddresses_CountryId_County_City_PostalCode_Street_HouseNumber_Floor_DoorNumber",
                table: "LodgingAddresses",
                columns: new[] { "CountryId", "County", "City", "PostalCode", "Street", "HouseNumber", "Floor", "DoorNumber" });
        }
    }
}
