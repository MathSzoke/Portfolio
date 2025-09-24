using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Database.Migrations.Application
{
    /// <inheritdoc />
    public partial class AddingNewColumnRateProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectAssets",
                schema: "portfolio");

            migrationBuilder.DropTable(
                name: "ProjectTags",
                schema: "portfolio");

            migrationBuilder.DropTable(
                name: "Showcases",
                schema: "portfolio");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "portfolio");

            migrationBuilder.DropIndex(
                name: "IX_Projects_Slug",
                schema: "portfolio",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DescriptionMarkdown",
                schema: "portfolio",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                schema: "portfolio",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Slug",
                schema: "portfolio",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Source",
                schema: "portfolio",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "RepoUrl",
                schema: "portfolio",
                table: "Projects",
                newName: "ThumbnailUrl");

            migrationBuilder.RenameColumn(
                name: "LiveUrl",
                schema: "portfolio",
                table: "Projects",
                newName: "RepoName");

            migrationBuilder.RenameColumn(
                name: "IconUrl",
                schema: "portfolio",
                table: "Projects",
                newName: "ProjectUrl");

            migrationBuilder.AlterColumn<float>(
                name: "Rating",
                schema: "portfolio",
                table: "Projects",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<int>(
                name: "RatingCount",
                schema: "portfolio",
                table: "Projects",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProjectRatings",
                schema: "portfolio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<float>(type: "real", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectRatings_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "portfolio",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRatings_ProjectId_UserId",
                schema: "portfolio",
                table: "ProjectRatings",
                columns: new[] { "ProjectId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectRatings",
                schema: "portfolio");

            migrationBuilder.DropColumn(
                name: "RatingCount",
                schema: "portfolio",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "ThumbnailUrl",
                schema: "portfolio",
                table: "Projects",
                newName: "RepoUrl");

            migrationBuilder.RenameColumn(
                name: "RepoName",
                schema: "portfolio",
                table: "Projects",
                newName: "LiveUrl");

            migrationBuilder.RenameColumn(
                name: "ProjectUrl",
                schema: "portfolio",
                table: "Projects",
                newName: "IconUrl");

            migrationBuilder.AlterColumn<float>(
                name: "Rating",
                schema: "portfolio",
                table: "Projects",
                type: "real",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real",
                oldDefaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionMarkdown",
                schema: "portfolio",
                table: "Projects",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                schema: "portfolio",
                table: "Projects",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                schema: "portfolio",
                table: "Projects",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Source",
                schema: "portfolio",
                table: "Projects",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ProjectAssets",
                schema: "portfolio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Kind = table.Column<string>(type: "text", nullable: false),
                    SortOrder = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    Title = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    Url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectAssets_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "portfolio",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Showcases",
                schema: "portfolio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
                    EndpointUrl = table.Column<string>(type: "text", nullable: true),
                    HealthUrl = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    ServiceName = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Showcases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Showcases_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "portfolio",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "portfolio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTags",
                schema: "portfolio",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTags", x => new { x.ProjectId, x.TagId });
                    table.ForeignKey(
                        name: "FK_ProjectTags_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "portfolio",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectTags_Tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "portfolio",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Slug",
                schema: "portfolio",
                table: "Projects",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAssets_ProjectId",
                schema: "portfolio",
                table: "ProjectAssets",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTags_TagId",
                schema: "portfolio",
                table: "ProjectTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Showcases_ProjectId",
                schema: "portfolio",
                table: "Showcases",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                schema: "portfolio",
                table: "Tags",
                column: "Name",
                unique: true);
        }
    }
}
