using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Lodgings.Migrations
{
    public partial class sp_AddUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"CREATE PROCEDURE [dbo].[AddUser]
                       @Username varchar(100),
                       @Password varchar(500),
                       @Email varchar(255),
                       @FirstName varchar(100),
                       @LastName varchar(100),
                       @RoleId smallint,
                       @ReturnValue int OUTPUT
                      AS
                       BEGIN
                            INSERT INTO [dbo].[Users](Username, Password, Email, FirstName, LastName, RoleId, IsDeleted, AddedAt, ModifiedAt)
                                               VALUES(@Username, @Password, @Email, @FirstName, @LastName, @RoleId, 0, GETDATE(), GETDATE());
                            SET @ReturnValue = SCOPE_IDENTITY();
                       END";
            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE [dbo].[AddUser]");
        }
    }
}
