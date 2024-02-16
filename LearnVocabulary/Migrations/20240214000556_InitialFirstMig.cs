using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnVocabulary.Migrations
{
    /// <inheritdoc />
    public partial class InitialFirstMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    WordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WordText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WordAudio = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.WordId);
                });

            migrationBuilder.CreateTable(
                name: "Means",
                columns: table => new
                {
                    MeaningId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PartOfSpeech = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Synonyms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Antonyms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Means", x => x.MeaningId);
                    table.ForeignKey(
                        name: "FK_Means_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "WordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Phonetics",
                columns: table => new
                {
                    PhoneticId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Audio = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phonetics", x => x.PhoneticId);
                    table.ForeignKey(
                        name: "FK_Phonetics_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "WordId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Definitions",
                columns: table => new
                {
                    DefinitionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DefinitionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MeaningId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Definitions", x => x.DefinitionId);
                    table.ForeignKey(
                        name: "FK_Definitions_Means_MeaningId",
                        column: x => x.MeaningId,
                        principalTable: "Means",
                        principalColumn: "MeaningId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Definitions_MeaningId",
                table: "Definitions",
                column: "MeaningId");

            migrationBuilder.CreateIndex(
                name: "IX_Means_WordId",
                table: "Means",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_Phonetics_WordId",
                table: "Phonetics",
                column: "WordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Definitions");

            migrationBuilder.DropTable(
                name: "Phonetics");

            migrationBuilder.DropTable(
                name: "Means");

            migrationBuilder.DropTable(
                name: "Words");
        }
    }
}
