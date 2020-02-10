using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Logging.Migrations
{
    public partial class init_logging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    ModifiedAt = table.Column<DateTime>(nullable: false, computedColumnSql: "getdate()"),
                    Timestamp = table.Column<DateTime>(nullable: false),
                    Message = table.Column<string>(maxLength: 1000, nullable: false),
                    Type = table.Column<string>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Log_PK", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");
        }
    }
}
