using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdventEmodal.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "password",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "CreatedAt", "Email", "IsAdmin", "PasswordHash", "UpdatedAt", "Username" },
                values: new object[] { 1, new DateTime(2024, 9, 15, 14, 45, 13, 94, DateTimeKind.Utc).AddTicks(4520), "admin@example.com", true, "$2a$11$hwaCFQKr.1u0/c0..nL2bOnVghMuWQA4H6jydSXoO.NYP4net6pwi", new DateTime(2024, 9, 15, 14, 45, 13, 94, DateTimeKind.Utc).AddTicks(4525), "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "password");
        }
    }
}
