using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UT01325MS3_GYMFEEMANAGEMENT.Migrations
{
    /// <inheritdoc />
    public partial class updatedv04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Payments",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Payments");
        }
    }
}
