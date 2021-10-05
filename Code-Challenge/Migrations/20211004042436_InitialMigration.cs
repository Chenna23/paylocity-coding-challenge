using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Code_Challenge.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblEmployee",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEmployee", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "tblDependent",
                columns: table => new
                {
                    DependentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RelationToEmployee = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblDependent", x => x.DependentId);
                    table.ForeignKey(
                        name: "FK_tblDependent_tblEmployee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "tblEmployee",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "tblEmployee",
                columns: new[] { "EmployeeId", "CreatedTime", "FirstName", "LastName", "ModifiedTime" },
                values: new object[] { 1, new DateTime(2021, 10, 4, 0, 24, 36, 30, DateTimeKind.Local).AddTicks(725), "Mark", "Adam", new DateTime(2021, 10, 4, 0, 24, 36, 36, DateTimeKind.Local).AddTicks(3606) });

            migrationBuilder.CreateIndex(
                name: "IX_tblDependent_EmployeeId",
                table: "tblDependent",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblDependent");

            migrationBuilder.DropTable(
                name: "tblEmployee");
        }
    }
}
