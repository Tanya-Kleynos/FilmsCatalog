using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FilmsCatalog.Migrations
{
    public partial class FixCreatorType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_AspNetUsers_CreatorId1",
                table: "Catalogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Films_AspNetUsers_CreatorId1",
                table: "Films");

            migrationBuilder.DropIndex(
                name: "IX_Films_CreatorId1",
                table: "Films");

            migrationBuilder.DropIndex(
                name: "IX_Catalogs_CreatorId1",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "CreatorId1",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "CreatorId1",
                table: "Catalogs");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Films",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Catalogs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_Films_CreatorId",
                table: "Films",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_CreatorId",
                table: "Catalogs",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_AspNetUsers_CreatorId",
                table: "Catalogs",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Films_AspNetUsers_CreatorId",
                table: "Films",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Catalogs_AspNetUsers_CreatorId",
                table: "Catalogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Films_AspNetUsers_CreatorId",
                table: "Films");

            migrationBuilder.DropIndex(
                name: "IX_Films_CreatorId",
                table: "Films");

            migrationBuilder.DropIndex(
                name: "IX_Catalogs_CreatorId",
                table: "Catalogs");

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "Films",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CreatorId1",
                table: "Films",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CreatorId",
                table: "Catalogs",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "CreatorId1",
                table: "Catalogs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Films_CreatorId1",
                table: "Films",
                column: "CreatorId1");

            migrationBuilder.CreateIndex(
                name: "IX_Catalogs_CreatorId1",
                table: "Catalogs",
                column: "CreatorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Catalogs_AspNetUsers_CreatorId1",
                table: "Catalogs",
                column: "CreatorId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Films_AspNetUsers_CreatorId1",
                table: "Films",
                column: "CreatorId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
