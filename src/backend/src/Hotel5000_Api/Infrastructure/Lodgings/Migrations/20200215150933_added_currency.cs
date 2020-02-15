using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Lodgings.Migrations
{
    public partial class added_currency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_LodgingAddresses_CountryId_County_City_PostalCode_Street",
                table: "LodgingAddresses",
                newName: "LodgingAddress_UQ");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Rooms",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_CurrencyId",
                table: "Rooms",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "Room_Currency_FK",
                table: "Rooms",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Room_Currency_FK",
                table: "Rooms");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_CurrencyId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Rooms");

            migrationBuilder.RenameIndex(
                name: "LodgingAddress_UQ",
                table: "LodgingAddresses",
                newName: "IX_LodgingAddresses_CountryId_County_City_PostalCode_Street");
        }
    }
}
