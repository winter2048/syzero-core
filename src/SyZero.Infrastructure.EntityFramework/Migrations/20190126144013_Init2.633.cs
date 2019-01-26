using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SyZero.Infrastructure.EntityFramework.Migrations
{
    public partial class Init2633 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Paw = table.Column<string>(maxLength: 200, nullable: true),
                    Mail = table.Column<string>(maxLength: 200, nullable: true),
                    Phone = table.Column<string>(maxLength: 200, nullable: true),
                    Headimg = table.Column<string>(maxLength: 1000, nullable: true),
                    Utype = table.Column<int>(nullable: false),
                    Sex = table.Column<string>(maxLength: 200, nullable: true),
                    AddTime = table.Column<DateTime>(nullable: false),
                    LastTime = table.Column<DateTime>(nullable: false),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
