using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace ZSB.Infrastructure.Apis.Login.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserModel",
                columns: table => new
                {
                    UniqueId = table.Column<Guid>(nullable: false),
                    AccountCreationDate = table.Column<DateTime>(nullable: false),
                    EmailAddress = table.Column<string>(nullable: false),
                    EmailConfirmationSent = table.Column<DateTime>(nullable: false),
                    IsEmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHashesCommaDelimited = table.Column<string>(nullable: true),
                    UniqueConfirmationCode = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserModel", x => x.UniqueId);
                    table.UniqueConstraint("AK_UserModel_EmailAddress", x => x.EmailAddress);
                    table.UniqueConstraint("AK_UserModel_Username", x => x.Username);
                });
            migrationBuilder.CreateTable(
                name: "UserActiveSessionModel",
                columns: table => new
                {
                    SessionKey = table.Column<string>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    OwnerUniqueId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActiveSessionModel", x => x.SessionKey);
                    table.ForeignKey(
                        name: "FK_UserActiveSessionModel_UserModel_OwnerUniqueId",
                        column: x => x.OwnerUniqueId,
                        principalTable: "UserModel",
                        principalColumn: "UniqueId");
                });
            migrationBuilder.CreateTable(
                name: "UserOwnedProductModel",
                columns: table => new
                {
                    ProductKey = table.Column<string>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    EditionId = table.Column<Guid>(nullable: false),
                    EditionName = table.Column<string>(nullable: true),
                    OwnerUniqueId = table.Column<Guid>(nullable: true),
                    ProductId = table.Column<Guid>(nullable: false),
                    ProductName = table.Column<string>(nullable: true),
                    RedemptionDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOwnedProductModel", x => x.ProductKey);
                    table.UniqueConstraint("AK_UserOwnedProductModel_EditionId", x => x.EditionId);
                    table.UniqueConstraint("AK_UserOwnedProductModel_ProductId", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_UserOwnedProductModel_UserModel_OwnerUniqueId",
                        column: x => x.OwnerUniqueId,
                        principalTable: "UserModel",
                        principalColumn: "UniqueId");
                });
            migrationBuilder.CreateTable(
                name: "UserServerTokenModel",
                columns: table => new
                {
                    ServerToken = table.Column<string>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: false),
                    OwnerUniqueId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserServerTokenModel", x => x.ServerToken);
                    table.ForeignKey(
                        name: "FK_UserServerTokenModel_UserModel_OwnerUniqueId",
                        column: x => x.OwnerUniqueId,
                        principalTable: "UserModel",
                        principalColumn: "UniqueId");
                });
            migrationBuilder.CreateIndex(
                name: "IX_UserActiveSessionModel_SessionKey",
                table: "UserActiveSessionModel",
                column: "SessionKey");
            migrationBuilder.CreateIndex(
                name: "IX_UserModel_EmailAddress",
                table: "UserModel",
                column: "EmailAddress");
            migrationBuilder.CreateIndex(
                name: "IX_UserModel_Username",
                table: "UserModel",
                column: "Username");
            migrationBuilder.CreateIndex(
                name: "IX_UserOwnedProductModel_ProductKey",
                table: "UserOwnedProductModel",
                column: "ProductKey");
            migrationBuilder.CreateIndex(
                name: "IX_UserServerTokenModel_ServerToken",
                table: "UserServerTokenModel",
                column: "ServerToken");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("UserActiveSessionModel");
            migrationBuilder.DropTable("UserOwnedProductModel");
            migrationBuilder.DropTable("UserServerTokenModel");
            migrationBuilder.DropTable("UserModel");
        }
    }
}
