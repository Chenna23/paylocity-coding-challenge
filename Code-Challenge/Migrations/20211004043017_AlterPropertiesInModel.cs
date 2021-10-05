using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Code_Challenge.Migrations
{
    public partial class AlterPropertiesInModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "tblEmployee");

            migrationBuilder.DropColumn(
                name: "ModifiedTime",
                table: "tblEmployee");

            migrationBuilder.DropColumn(
                name: "CreatedTime",
                table: "tblDependent");

            migrationBuilder.DropColumn(
                name: "ModifiedTime",
                table: "tblDependent");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "tblEmployee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedTime",
                table: "tblEmployee",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedTime",
                table: "tblDependent",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedTime",
                table: "tblDependent",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "tblEmployee",
                keyColumn: "EmployeeId",
                keyValue: 1,
                columns: new[] { "CreatedTime", "ModifiedTime" },
                values: new object[] { new DateTime(2021, 10, 4, 0, 24, 36, 30, DateTimeKind.Local).AddTicks(725), new DateTime(2021, 10, 4, 0, 24, 36, 36, DateTimeKind.Local).AddTicks(3606) });
        }
    }
}
