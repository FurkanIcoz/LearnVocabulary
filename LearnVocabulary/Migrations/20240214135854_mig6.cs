using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnVocabulary.Migrations
{
    /// <inheritdoc />
    public partial class mig6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnknownWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnglistText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TurkishText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnknownWords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WordsSentences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sentence = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnknownWordId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordsSentences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordsSentences_UnknownWords_UnknownWordId",
                        column: x => x.UnknownWordId,
                        principalTable: "UnknownWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WordsSentences_UnknownWordId",
                table: "WordsSentences",
                column: "UnknownWordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WordsSentences");

            migrationBuilder.DropTable(
                name: "UnknownWords");
        }
    }
}
