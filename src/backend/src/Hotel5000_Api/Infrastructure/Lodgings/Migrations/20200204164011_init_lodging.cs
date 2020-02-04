using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Lodgings.Migrations
{
    public partial class init_lodging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    ModifiedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    Code = table.Column<string>(maxLength: 2, nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Country_PK", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    ModifiedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    Name = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PaymentType_PK", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    ModifiedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    Name = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Role_PK", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserReservations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    ModifiedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    PaymentTypeId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("UserReservation_PK", x => x.Id);
                    table.ForeignKey(
                        name: "UserReservation_PaymentType_FK",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    ModifiedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    Username = table.Column<string>(maxLength: 100, nullable: false),
                    Password = table.Column<string>(maxLength: 500, nullable: false),
                    Email = table.Column<string>(maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(maxLength: 100, nullable: false),
                    LastName = table.Column<string>(maxLength: 100, nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_PK", x => x.Id);
                    table.ForeignKey(
                        name: "User_Role_FK",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApprovingData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    ModifiedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    IdentityNumber = table.Column<string>(maxLength: 8, nullable: true),
                    TaxNumber = table.Column<string>(maxLength: 13, nullable: true),
                    RegistrationNumber = table.Column<string>(maxLength: 12, nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ApprovingData_PK", x => x.Id);
                    table.ForeignKey(
                        name: "ApprovingData_User_FK",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    ModifiedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    MobileNumber = table.Column<string>(maxLength: 13, nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Contact_PK", x => x.Id);
                    table.ForeignKey(
                        name: "Contact_User_FK",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lodgings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    ModifiedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Lodging_PK", x => x.Id);
                    table.ForeignKey(
                        name: "Lodgind_User_FK",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    ModifiedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    RefreshToken = table.Column<string>(maxLength: 45, nullable: true),
                    UsableFrom = table.Column<DateTime>(nullable: false),
                    ExpiresAt = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Token_PK", x => x.Id);
                    table.ForeignKey(
                        name: "Token_User_Id_FK",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LodgingAddresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    ModifiedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    County = table.Column<string>(maxLength: 100, nullable: false),
                    City = table.Column<string>(maxLength: 100, nullable: false),
                    PostalCode = table.Column<string>(maxLength: 10, nullable: false),
                    Street = table.Column<string>(maxLength: 100, nullable: false),
                    HouseNumber = table.Column<string>(maxLength: 10, nullable: false),
                    Floor = table.Column<string>(maxLength: 10, nullable: false),
                    DoorNumber = table.Column<string>(maxLength: 10, nullable: false),
                    CountryId = table.Column<int>(nullable: false),
                    LodgingId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("LodgingAddress_PK", x => x.Id);
                    table.UniqueConstraint("AK_LodgingAddresses_CountryId_County_City_PostalCode_Street_HouseNumber_Floor_DoorNumber", x => new { x.CountryId, x.County, x.City, x.PostalCode, x.Street, x.HouseNumber, x.Floor, x.DoorNumber });
                    table.ForeignKey(
                        name: "LodgingAddress_Country_FK",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "LodgingAddress_Lodging_FK",
                        column: x => x.LodgingId,
                        principalTable: "Lodgings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    ModifiedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    AdultCapacity = table.Column<int>(nullable: false),
                    ChildrenCapacity = table.Column<int>(nullable: false),
                    LodgingId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Room_PK", x => x.Id);
                    table.ForeignKey(
                        name: "Room_Lodging_FK",
                        column: x => x.LodgingId,
                        principalTable: "Lodgings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservationWindows",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    ModifiedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    From = table.Column<DateTime>(nullable: false),
                    To = table.Column<DateTime>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    RoomId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ReservationWindow_PK", x => x.Id);
                    table.ForeignKey(
                        name: "ReservationWindow_Room_FK",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    ModifiedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    ReservedFrom = table.Column<DateTime>(nullable: false),
                    ReservedTo = table.Column<DateTime>(nullable: false),
                    ReservationWindowId = table.Column<int>(nullable: false),
                    UserReservationId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Reservation_PK", x => x.Id);
                    table.ForeignKey(
                        name: "Reservation_ReservationWindow_FK",
                        column: x => x.ReservationWindowId,
                        principalTable: "ReservationWindows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "Reservation_UserReservation_FK",
                        column: x => x.UserReservationId,
                        principalTable: "UserReservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ApprovingData_IdentityNumber_UQ",
                table: "ApprovingData",
                column: "IdentityNumber",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "ApprovingData_RegistrationNumber_UQ",
                table: "ApprovingData",
                column: "RegistrationNumber",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "ApprovingData_TaxNumber_UQ",
                table: "ApprovingData",
                column: "TaxNumber",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovingData_UserId",
                table: "ApprovingData",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Contact_MobileNumber_UQ",
                table: "Contacts",
                column: "MobileNumber",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_UserId",
                table: "Contacts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "Country_CountyCode_UQ",
                table: "Countries",
                column: "Code",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "Country_CountryName_UQ",
                table: "Countries",
                column: "Name",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_LodgingAddresses_LodgingId",
                table: "LodgingAddresses",
                column: "LodgingId");

            migrationBuilder.CreateIndex(
                name: "IX_Lodgings_UserId",
                table: "Lodgings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "PaymentType_Name_UQ",
                table: "PaymentTypes",
                column: "Name",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ReservationWindowId",
                table: "Reservations",
                column: "ReservationWindowId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserReservationId",
                table: "Reservations",
                column: "UserReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationWindows_RoomId",
                table: "ReservationWindows",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "Role_Name_UQ",
                table: "Roles",
                column: "Name",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_LodgingId",
                table: "Rooms",
                column: "LodgingId");

            migrationBuilder.CreateIndex(
                name: "IX_Tokens_UserId",
                table: "Tokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReservations_PaymentTypeId",
                table: "UserReservations",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "User_Email_UQ",
                table: "Users",
                column: "Email",
                unique: true,
                filter: "IsDeleted = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "User_Username_UQ",
                table: "Users",
                column: "Username",
                unique: true,
                filter: "IsDeleted = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApprovingData");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "LodgingAddresses");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Tokens");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "ReservationWindows");

            migrationBuilder.DropTable(
                name: "UserReservations");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "PaymentTypes");

            migrationBuilder.DropTable(
                name: "Lodgings");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
