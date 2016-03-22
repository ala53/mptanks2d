using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace ZSB.Infrastructure.Apis.Login.Migrations
{
    public partial class RemoveUnnecessaryKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(name: "AK_UserModel_EmailAddress", table: "UserModel");
            migrationBuilder.DropUniqueConstraint(name: "AK_UserModel_Username", table: "UserModel");
            migrationBuilder.DropIndex(name: "IX_UserModel_Username", table: "UserModel");
            migrationBuilder.DropIndex(name: "IX_UserModel_EmailAddress", table: "UserModel");
            migrationBuilder.CreateIndex(
                name: "IX_UserModel_Username",
                table: "UserModel",
                column: "Username",
                unique: true);
            migrationBuilder.CreateIndex(
                name: "IX_UserModel_EmailAddress",
                table: "UserModel",
                column: "EmailAddress",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_UserModel_Username", table: "UserModel");
            migrationBuilder.DropIndex(name: "IX_UserModel_EmailAddress", table: "UserModel");
            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserModel_EmailAddress",
                table: "UserModel",
                column: "EmailAddress");
            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserModel_Username",
                table: "UserModel",
                column: "Username");
            migrationBuilder.CreateIndex(
                name: "IX_UserModel_Username",
                table: "UserModel",
                column: "Username");
            migrationBuilder.CreateIndex(
                name: "IX_UserModel_EmailAddress",
                table: "UserModel",
                column: "EmailAddress");
        }
    }
}
