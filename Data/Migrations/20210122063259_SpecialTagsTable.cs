﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace WebStore.Data.Migrations
{
    public partial class SpecialTagsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProductsTypes",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SpecialTags",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 30, nullable: false),                  
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialTags", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
