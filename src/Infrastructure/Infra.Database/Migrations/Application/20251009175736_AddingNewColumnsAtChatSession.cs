using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Database.Migrations.Application
{
    /// <inheritdoc />
    public partial class AddingNewColumnsAtChatSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                schema: "portfolio",
                table: "ChatSessions");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "portfolio",
                table: "ChatSessions");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                schema: "portfolio",
                table: "ChatSessions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "VisitorEmail",
                schema: "portfolio",
                table: "ChatSessions",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VisitorId",
                schema: "portfolio",
                table: "ChatSessions",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisitorName",
                schema: "portfolio",
                table: "ChatSessions",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SenderUserId",
                schema: "portfolio",
                table: "ChatMessages",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_OwnerId",
                schema: "portfolio",
                table: "ChatSessions",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_VisitorEmail",
                schema: "portfolio",
                table: "ChatSessions",
                column: "VisitorEmail");

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_VisitorId",
                schema: "portfolio",
                table: "ChatSessions",
                column: "VisitorId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_SenderUserId",
                schema: "portfolio",
                table: "ChatMessages",
                column: "SenderUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ChatSessions_OwnerId",
                schema: "portfolio",
                table: "ChatSessions");

            migrationBuilder.DropIndex(
                name: "IX_ChatSessions_VisitorEmail",
                schema: "portfolio",
                table: "ChatSessions");

            migrationBuilder.DropIndex(
                name: "IX_ChatSessions_VisitorId",
                schema: "portfolio",
                table: "ChatSessions");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_SenderUserId",
                schema: "portfolio",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                schema: "portfolio",
                table: "ChatSessions");

            migrationBuilder.DropColumn(
                name: "VisitorEmail",
                schema: "portfolio",
                table: "ChatSessions");

            migrationBuilder.DropColumn(
                name: "VisitorId",
                schema: "portfolio",
                table: "ChatSessions");

            migrationBuilder.DropColumn(
                name: "VisitorName",
                schema: "portfolio",
                table: "ChatSessions");

            migrationBuilder.DropColumn(
                name: "SenderUserId",
                schema: "portfolio",
                table: "ChatMessages");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "portfolio",
                table: "ChatSessions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "portfolio",
                table: "ChatSessions",
                type: "text",
                nullable: true);
        }
    }
}
