using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MemoApp.Model.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    ConfigId = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configs", x => x.ConfigId);
                });

            migrationBuilder.CreateTable(
                name: "EachTasks",
                columns: table => new
                {
                    EachTaskId = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    DeadLine = table.Column<DateTimeOffset>(nullable: false),
                    PlanDate = table.Column<DateTimeOffset>(nullable: false),
                    RegisteredDate = table.Column<DateTimeOffset>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    ParentEachTaskId = table.Column<string>(nullable: true),
                    CompleteFlag = table.Column<bool>(nullable: false),
                    StartedFlag = table.Column<bool>(nullable: false),
                    Rank = table.Column<int>(nullable: false),
                    ValidFlag = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EachTasks", x => x.EachTaskId);
                });

            migrationBuilder.CreateTable(
                name: "Memos",
                columns: table => new
                {
                    MemoId = table.Column<string>(nullable: false),
                    CreateTime = table.Column<DateTimeOffset>(nullable: false),
                    UpdateTime = table.Column<DateTimeOffset>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    EachTaskId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memos", x => x.MemoId);
                    table.ForeignKey(
                        name: "FK_Memos_EachTasks_EachTaskId",
                        column: x => x.EachTaskId,
                        principalTable: "EachTasks",
                        principalColumn: "EachTaskId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TimeInfos",
                columns: table => new
                {
                    TimeInfoId = table.Column<string>(nullable: false),
                    Start = table.Column<DateTimeOffset>(nullable: false),
                    Stop = table.Column<DateTimeOffset>(nullable: false),
                    EachTaskId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeInfos", x => x.TimeInfoId);
                    table.ForeignKey(
                        name: "FK_TimeInfos_EachTasks_EachTaskId",
                        column: x => x.EachTaskId,
                        principalTable: "EachTasks",
                        principalColumn: "EachTaskId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Memos_EachTaskId",
                table: "Memos",
                column: "EachTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeInfos_EachTaskId",
                table: "TimeInfos",
                column: "EachTaskId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropTable(
                name: "Memos");

            migrationBuilder.DropTable(
                name: "TimeInfos");

            migrationBuilder.DropTable(
                name: "EachTasks");
        }
    }
}
