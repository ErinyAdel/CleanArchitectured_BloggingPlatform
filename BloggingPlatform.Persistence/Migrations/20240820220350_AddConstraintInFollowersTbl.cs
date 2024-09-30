using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloggingPlatform.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddConstraintInFollowersTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE Followers ADD CONSTRAINT CK_Followers_NoSelfFollow CHECK (FollowerUserId <> FollowedUserId)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("ALTER TABLE Followers DROP CONSTRAINT CK_Followers_NoSelfFollow");
        }
    }
}
