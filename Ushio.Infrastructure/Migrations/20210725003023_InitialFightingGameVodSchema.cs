using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Ushio.Infrastructure.Migrations
{
    public partial class InitialFightingGameVodSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FightingGameVods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OriginalTitle = table.Column<string>(type: "text", nullable: true),
                    VideoId = table.Column<string>(type: "text", nullable: true),
                    GameName = table.Column<int>(type: "integer", nullable: false),
                    SourceChannel = table.Column<string>(type: "text", nullable: true),
                    Player1 = table.Column<string>(type: "text", nullable: true),
                    Player2 = table.Column<string>(type: "text", nullable: true),
                    CharacterP1 = table.Column<string>(type: "text", nullable: true),
                    CharacterP2 = table.Column<string>(type: "text", nullable: true),
                    DateUploaded = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    DateAddedToRepo = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FightingGameVods", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FightingGameVods");
        }
    }
}
