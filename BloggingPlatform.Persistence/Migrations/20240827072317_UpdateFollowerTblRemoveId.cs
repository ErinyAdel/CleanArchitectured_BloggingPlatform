using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BloggingPlatform.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFollowerTblRemoveId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Followers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Followers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
