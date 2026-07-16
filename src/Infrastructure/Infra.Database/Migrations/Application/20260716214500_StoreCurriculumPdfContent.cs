using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Database.Migrations.Application
{
    /// <inheritdoc />
    public partial class StoreCurriculumPdfContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                schema: "portfolio",
                table: "CurriculumAssets",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048);

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                schema: "portfolio",
                table: "CurriculumAssets",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "FileContent",
                schema: "portfolio",
                table: "CurriculumAssets",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                schema: "portfolio",
                table: "CurriculumAssets",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                schema: "portfolio",
                table: "CurriculumAssets");

            migrationBuilder.DropColumn(
                name: "FileContent",
                schema: "portfolio",
                table: "CurriculumAssets");

            migrationBuilder.DropColumn(
                name: "FileName",
                schema: "portfolio",
                table: "CurriculumAssets");

            migrationBuilder.Sql("""UPDATE portfolio."CurriculumAssets" SET "Url" = '' WHERE "Url" IS NULL""");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                schema: "portfolio",
                table: "CurriculumAssets",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048,
                oldNullable: true);
        }
    }
}
