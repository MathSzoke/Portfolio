using Infra.Database.Portfolio;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Database.Migrations.Application
{
    /// <inheritdoc />
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260716224500_AddExperienceDateRange")]
    public partial class AddExperienceDateRange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EndDate",
                schema: "portfolio",
                table: "ExperienceItems",
                type: "character varying(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsPresent",
                schema: "portfolio",
                table: "ExperienceItems",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                schema: "portfolio",
                table: "ExperienceItems");

            migrationBuilder.DropColumn(
                name: "IsPresent",
                schema: "portfolio",
                table: "ExperienceItems");
        }
    }
}
