using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnVocabulary.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Definitions_Means_MeaningId",
                table: "Definitions");

            migrationBuilder.DropTable(
                name: "Means");

            migrationBuilder.RenameColumn(
                name: "WordId",
                table: "Words",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "PhoneticId",
                table: "Phonetics",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "DefinitionText",
                table: "Definitions",
                newName: "Synonyms");

            migrationBuilder.RenameColumn(
                name: "DefinitionId",
                table: "Definitions",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Phonetic",
                table: "Words",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Phonetics",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Antonyms",
                table: "Definitions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Definition",
                table: "Definitions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Example",
                table: "Definitions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Meanings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WordId = table.Column<int>(type: "int", nullable: false),
                    PartOfSpeech = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meanings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meanings_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meanings_WordId",
                table: "Meanings",
                column: "WordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Definitions_Meanings_MeaningId",
                table: "Definitions",
                column: "MeaningId",
                principalTable: "Meanings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Definitions_Meanings_MeaningId",
                table: "Definitions");

            migrationBuilder.DropTable(
                name: "Meanings");

            migrationBuilder.DropColumn(
                name: "Phonetic",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Phonetics");

            migrationBuilder.DropColumn(
                name: "Antonyms",
                table: "Definitions");

            migrationBuilder.DropColumn(
                name: "Definition",
                table: "Definitions");

            migrationBuilder.DropColumn(
                name: "Example",
                table: "Definitions");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Words",
                newName: "WordId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Phonetics",
                newName: "PhoneticId");

            migrationBuilder.RenameColumn(
                name: "Synonyms",
                table: "Definitions",
                newName: "DefinitionText");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Definitions",
                newName: "DefinitionId");

            migrationBuilder.CreateTable(
                name: "Means",
                columns: table => new
                {
                    MeaningId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WordId = table.Column<int>(type: "int", nullable: false),
                    Antonyms = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PartOfSpeech = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Synonyms = table.Column<string>(type: "nvarchar(max)", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Means_WordId",
                table: "Means",
                column: "WordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Definitions_Means_MeaningId",
                table: "Definitions",
                column: "MeaningId",
                principalTable: "Means",
                principalColumn: "MeaningId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
