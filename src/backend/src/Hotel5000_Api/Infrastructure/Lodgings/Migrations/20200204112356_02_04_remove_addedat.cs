using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Lodgings.Migrations
{
    public partial class _02_04_remove_addedat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedAt",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AddedAt",
                table: "UserReservations");

            migrationBuilder.DropColumn(
                name: "AddedAt",
                table: "Tokens");

            migrationBuilder.DropColumn(
                name: "AddedAt",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "AddedAt",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "AddedAt",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "AddedAt",
                table: "Reser");

            migrationBuilder.DropColumn(
                name: "AddedAt",
                table: "PaymentTypes");

            migrationBuilder.DropColumn(
                name: "AddedAt",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "AddedAt",
                table: "Lodgings");

            migrationBuilder.DropColumn(
                name: "AddedAt",
                table: "LodgingAddresses");

            migrationBuilder.DropColumn(
                name: "AddedAt",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "AddedAt",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "AddedAt",
                table: "ApprovingData");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AddedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedAt",
                table: "UserReservations",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedAt",
                table: "Tokens",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedAt",
                table: "Rooms",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedAt",
                table: "Roles",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedAt",
                table: "Reservations",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedAt",
                table: "Reser",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedAt",
                table: "PaymentTypes",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedAt",
                table: "Log",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedAt",
                table: "Lodgings",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedAt",
                table: "LodgingAddresses",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedAt",
                table: "Countries",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedAt",
                table: "Contacts",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "getdate()");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddedAt",
                table: "ApprovingData",
                type: "datetime2",
                nullable: false,
                computedColumnSql: "getdate()");
        }
    }
}
