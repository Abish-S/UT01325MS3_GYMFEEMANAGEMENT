using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UT01325MS3_GYMFEEMANAGEMENT.Migrations
{
    /// <inheritdoc />
    public partial class updatedv02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "TrainingPrograms",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "TrainingPrograms");
        }
    }
}
