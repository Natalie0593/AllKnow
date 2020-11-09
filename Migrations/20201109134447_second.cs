using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogHost.Migrations
{
    public partial class second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Like",
                table: "Publications");

            migrationBuilder.AddColumn<byte[]>(
                name: "AvatarPost",
                table: "Publications",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "AvatarPost2",
                table: "Publications",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LikePost",
                table: "Publications",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "Avatar",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarPost",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "AvatarPost2",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "LikePost",
                table: "Publications");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Like",
                table: "Publications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
