using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UT01325MS3_GYMFEEMANAGEMENT.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlertDate",
                table: "Alerts");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Alerts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsResolved",
                table: "Alerts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Alerts");

            migrationBuilder.DropColumn(
                name: "IsResolved",
                table: "Alerts");

            migrationBuilder.AddColumn<DateTime>(
                name: "AlertDate",
                table: "Alerts",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");
        }
    }
}
