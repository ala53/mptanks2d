using System;
using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;

namespace ZSB.Infrastructure.Apis.Login.Migrations
{
    public partial class RemoveProductAndEditionNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "EditionName", table: "UserOwnedProductModel");
            migrationBuilder.DropColumn(name: "ProductName", table: "UserOwnedProductModel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EditionName",
                table: "UserOwnedProductModel",
                nullable: true);
            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "UserOwnedProductModel",
                nullable: true);
        }
    }
}
