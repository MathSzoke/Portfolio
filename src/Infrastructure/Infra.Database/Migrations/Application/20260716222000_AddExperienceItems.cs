using System;
using Infra.Database.Portfolio;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Database.Migrations.Application
{
    /// <inheritdoc />
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260716222000_AddExperienceItems")]
    public partial class AddExperienceItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExperienceItems",
                schema: "portfolio",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Company = table.Column<string>(type: "character varying(160)", maxLength: 160, nullable: false),
                    LogoUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    RolePtBr = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    RoleEnUs = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    PeriodPtBr = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    PeriodEnUs = table.Column<string>(type: "character varying(180)", maxLength: 180, nullable: false),
                    LocationPtBr = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    LocationEnUs = table.Column<string>(type: "character varying(220)", maxLength: 220, nullable: false),
                    DescriptionPtBr = table.Column<string[]>(type: "text[]", nullable: false),
                    DescriptionEnUs = table.Column<string[]>(type: "text[]", nullable: false),
                    Techs = table.Column<string[]>(type: "text[]", nullable: false),
                    StartDate = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    SortOrder = table.Column<int>(type: "integer", nullable: false),
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
                    table.PrimaryKey("PK_ExperienceItems", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExperienceItems_SortOrder",
                schema: "portfolio",
                table: "ExperienceItems",
                column: "SortOrder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExperienceItems",
                schema: "portfolio");
        }
    }
}
