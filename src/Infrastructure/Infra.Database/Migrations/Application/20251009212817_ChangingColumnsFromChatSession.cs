using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Database.Migrations.Application
{
    /// <inheritdoc />
    public partial class ChangingColumnsFromChatSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ChatSessions_Status_UpdatedAt",
                schema: "portfolio",
                table: "ChatSessions");

            migrationBuilder.DropIndex(
                name: "IX_ChatSessions_VisitorEmail",
                schema: "portfolio",
                table: "ChatSessions");

            migrationBuilder.DropColumn(
                name: "ClientIp",
                schema: "portfolio",
                table: "ChatSessions");

            migrationBuilder.DropColumn(
                name: "ConsentEmail",
                schema: "portfolio",
                table: "ChatSessions");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                schema: "portfolio",
                table: "ChatSessions");

            migrationBuilder.DropColumn(
                name: "VisitorEmail",
                schema: "portfolio",
                table: "ChatSessions");

            migrationBuilder.DropColumn(
                name: "VisitorName",
                schema: "portfolio",
                table: "ChatSessions");

            migrationBuilder.RenameColumn(
                name: "VisitorId",
                schema: "portfolio",
                table: "ChatSessions",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "OwnerId",
                schema: "portfolio",
                table: "ChatSessions",
                newName: "RecipientId");

            migrationBuilder.RenameColumn(
                name: "LastSeenAt",
                schema: "portfolio",
                table: "ChatSessions",
                newName: "LastSenderSeenAt");

            migrationBuilder.RenameColumn(
                name: "LastAgentSeenAt",
                schema: "portfolio",
                table: "ChatSessions",
                newName: "LastRecipientSeenAt");

            migrationBuilder.RenameIndex(
                name: "IX_ChatSessions_VisitorId",
                schema: "portfolio",
                table: "ChatSessions",
                newName: "IX_ChatSessions_SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatSessions_OwnerId",
                schema: "portfolio",
                table: "ChatSessions",
                newName: "IX_ChatSessions_RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_SenderId_RecipientId_Status",
                schema: "portfolio",
                table: "ChatSessions",
                columns: new[] { "SenderId", "RecipientId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_UpdatedAt",
                schema: "portfolio",
                table: "ChatSessions",
                column: "UpdatedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ChatSessions_SenderId_RecipientId_Status",
                schema: "portfolio",
                table: "ChatSessions");

            migrationBuilder.DropIndex(
                name: "IX_ChatSessions_UpdatedAt",
                schema: "portfolio",
                table: "ChatSessions");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                schema: "portfolio",
                table: "ChatSessions",
                newName: "VisitorId");

            migrationBuilder.RenameColumn(
                name: "RecipientId",
                schema: "portfolio",
                table: "ChatSessions",
                newName: "OwnerId");

            migrationBuilder.RenameColumn(
                name: "LastSenderSeenAt",
                schema: "portfolio",
                table: "ChatSessions",
                newName: "LastSeenAt");

            migrationBuilder.RenameColumn(
                name: "LastRecipientSeenAt",
                schema: "portfolio",
                table: "ChatSessions",
                newName: "LastAgentSeenAt");

            migrationBuilder.RenameIndex(
                name: "IX_ChatSessions_SenderId",
                schema: "portfolio",
                table: "ChatSessions",
                newName: "IX_ChatSessions_VisitorId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatSessions_RecipientId",
                schema: "portfolio",
                table: "ChatSessions",
                newName: "IX_ChatSessions_OwnerId");

            migrationBuilder.AddColumn<string>(
                name: "ClientIp",
                schema: "portfolio",
                table: "ChatSessions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ConsentEmail",
                schema: "portfolio",
                table: "ChatSessions",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                schema: "portfolio",
                table: "ChatSessions",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisitorEmail",
                schema: "portfolio",
                table: "ChatSessions",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VisitorName",
                schema: "portfolio",
                table: "ChatSessions",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_Status_UpdatedAt",
                schema: "portfolio",
                table: "ChatSessions",
                columns: new[] { "Status", "UpdatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ChatSessions_VisitorEmail",
                schema: "portfolio",
                table: "ChatSessions",
                column: "VisitorEmail");
        }
    }
}
