using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnVocabulary.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LicenseId",
                table: "Words",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SourceUrls",
                table: "Words",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "LicanseInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicanseInfos", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Words_LicenseId",
                table: "Words",
                column: "LicenseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Words_LicanseInfos_LicenseId",
                table: "Words",
                column: "LicenseId",
                principalTable: "LicanseInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Words_LicanseInfos_LicenseId",
                table: "Words");

            migrationBuilder.DropTable(
                name: "LicanseInfos");

            migrationBuilder.DropIndex(
                name: "IX_Words_LicenseId",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "LicenseId",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "SourceUrls",
                table: "Words");
        }
    }
}
