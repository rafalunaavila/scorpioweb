using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace scorpioweb.Migrations
{
    public partial class probando : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Supervisor",
                table: "persona",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Supervisor",
                table: "persona");
        }
    }
}
