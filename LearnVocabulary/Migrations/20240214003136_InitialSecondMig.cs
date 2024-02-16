using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnVocabulary.Migrations
{
    /// <inheritdoc />
    public partial class InitialSecondMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WordAudio",
                table: "Words");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "WordAudio",
                table: "Words",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
