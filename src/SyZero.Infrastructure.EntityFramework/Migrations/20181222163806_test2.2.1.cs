using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SyZero.Infrastructure.EntityFramework.Migrations
{
    public partial class test221 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Article",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    Ctid = table.Column<int>(nullable: false),
                    Author = table.Column<string>(maxLength: 200, nullable: true),
                    Img = table.Column<string>(maxLength: 200, nullable: true),
                    AddTime = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    Hot = table.Column<int>(nullable: false),
                    IsDisplay = table.Column<int>(nullable: false),
                    L01 = table.Column<string>(nullable: true),
                    L02 = table.Column<string>(nullable: true),
                    L03 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Article", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categorys",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(maxLength: 200, nullable: false),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Parentid = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorys", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Userid = table.Column<int>(nullable: false),
                    Artid = table.Column<int>(nullable: false),
                    Content = table.Column<string>(maxLength: 1000, nullable: true),
                    AddTime = table.Column<DateTime>(nullable: false),
                    Pabulous = table.Column<int>(nullable: false),
                    Parent = table.Column<int>(nullable: false),
                    L01 = table.Column<string>(nullable: true),
                    L02 = table.Column<string>(nullable: true),
                    L03 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Configure",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(maxLength: 200, nullable: true),
                    Content = table.Column<string>(maxLength: 1000, nullable: true),
                    Other = table.Column<string>(maxLength: 200, nullable: true),
                    L01 = table.Column<string>(nullable: true),
                    L02 = table.Column<string>(nullable: true),
                    L03 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configure", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FriendLin",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(maxLength: 200, nullable: true),
                    Href = table.Column<string>(maxLength: 1000, nullable: true),
                    Category = table.Column<string>(maxLength: 200, nullable: true),
                    L01 = table.Column<string>(nullable: true),
                    L02 = table.Column<string>(nullable: true),
                    L03 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendLin", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messaged",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Userid = table.Column<int>(nullable: false),
                    Content = table.Column<string>(maxLength: 1000, nullable: true),
                    AddTime = table.Column<DateTime>(nullable: false),
                    Parent = table.Column<int>(nullable: false),
                    L01 = table.Column<string>(nullable: true),
                    L02 = table.Column<string>(nullable: true),
                    L03 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messaged", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Artid = table.Column<int>(nullable: false),
                    Keyword = table.Column<string>(maxLength: 200, nullable: true),
                    Describe = table.Column<string>(maxLength: 200, nullable: true),
                    L01 = table.Column<string>(nullable: true),
                    L02 = table.Column<string>(nullable: true),
                    L03 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeAxis",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Releasetime = table.Column<DateTime>(nullable: false),
                    Releasetitle = table.Column<string>(maxLength: 200, nullable: true),
                    Brief = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeAxis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tool",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: true),
                    Ctid = table.Column<int>(nullable: false),
                    Url = table.Column<string>(maxLength: 1000, nullable: true),
                    Tqm = table.Column<string>(maxLength: 200, nullable: true),
                    L01 = table.Column<string>(nullable: true),
                    L02 = table.Column<string>(nullable: true),
                    L03 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tool", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
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
                    State = table.Column<int>(nullable: false),
                    L01 = table.Column<string>(nullable: true),
                    L02 = table.Column<string>(nullable: true),
                    L03 = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 200, nullable: false),
                    Authority = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserType", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Article");

            migrationBuilder.DropTable(
                name: "Categorys");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Configure");

            migrationBuilder.DropTable(
                name: "FriendLin");

            migrationBuilder.DropTable(
                name: "Messaged");

            migrationBuilder.DropTable(
                name: "Seo");

            migrationBuilder.DropTable(
                name: "TimeAxis");

            migrationBuilder.DropTable(
                name: "Tool");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "UserType");
        }
    }
}
