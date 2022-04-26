using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Logcast.Recruitment.DataAccess.Migrations
{
    public partial class AddAudioEntityAndAudioMetadataEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Audios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SaveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AudioFileName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AudioMetadatas",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AudioFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Artist = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bitrate = table.Column<int>(type: "int", nullable: false),
                    Codec = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudioMetadatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AudioMetadatas_Audios_AudioFileId",
                        column: x => x.AudioFileId,
                        principalTable: "Audios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AudioMetadatas_AudioFileId",
                table: "AudioMetadatas",
                column: "AudioFileId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AudioMetadatas");

            migrationBuilder.DropTable(
                name: "Audios");
        }
    }
}
