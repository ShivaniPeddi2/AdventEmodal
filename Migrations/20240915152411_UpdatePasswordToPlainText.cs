using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdventEmodal.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePasswordToPlainText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Users",
                newName: "Password");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Password", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 9, 15, 15, 24, 10, 617, DateTimeKind.Utc).AddTicks(4203), "admin_password", new DateTime(2024, 9, 15, 15, 24, 10, 617, DateTimeKind.Utc).AddTicks(4204) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 9, 15, 14, 45, 13, 94, DateTimeKind.Utc).AddTicks(4520), "$2a$11$hwaCFQKr.1u0/c0..nL2bOnVghMuWQA4H6jydSXoO.NYP4net6pwi", new DateTime(2024, 9, 15, 14, 45, 13, 94, DateTimeKind.Utc).AddTicks(4525) });
        }
    }
}
