using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UT01325MS3_GYMFEEMANAGEMENT.Migrations
{
    /// <inheritdoc />
    public partial class updatedv09 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "Members");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordSalt",
                table: "Members",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
