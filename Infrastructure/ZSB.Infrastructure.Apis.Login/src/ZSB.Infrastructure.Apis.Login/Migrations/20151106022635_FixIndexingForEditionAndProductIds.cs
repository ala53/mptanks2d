using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace ZSB.Infrastructure.Apis.Login.Migrations
{
    public partial class FixIndexingForEditionAndProductIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(name: "AK_UserOwnedProductModel_EditionId", table: "UserOwnedProductModel");
            migrationBuilder.DropUniqueConstraint(name: "AK_UserOwnedProductModel_ProductId", table: "UserOwnedProductModel");
            migrationBuilder.DropColumn(name: "DisplayName", table: "UserOwnedProductModel");
            migrationBuilder.CreateIndex(
                name: "IX_UserOwnedProductModel_EditionId",
                table: "UserOwnedProductModel",
                column: "EditionId");
            migrationBuilder.CreateIndex(
                name: "IX_UserOwnedProductModel_ProductId",
                table: "UserOwnedProductModel",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(name: "IX_UserOwnedProductModel_EditionId", table: "UserOwnedProductModel");
            migrationBuilder.DropIndex(name: "IX_UserOwnedProductModel_ProductId", table: "UserOwnedProductModel");
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "UserOwnedProductModel",
                nullable: true);
            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserOwnedProductModel_EditionId",
                table: "UserOwnedProductModel",
                column: "EditionId");
            migrationBuilder.AddUniqueConstraint(
                name: "AK_UserOwnedProductModel_ProductId",
                table: "UserOwnedProductModel",
                column: "ProductId");
        }
    }
}
