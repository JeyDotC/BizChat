using Microsoft.EntityFrameworkCore.Migrations;

namespace Bizchat.Web.Migrations
{
    public partial class BizChat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatRoomReceivedMessages",
                columns: table => new
                {
                    ChatRoomId = table.Column<int>(nullable: false),
                    ChatMessageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRoomReceivedMessages", x => new { x.ChatRoomId, x.ChatMessageId });
                    table.ForeignKey(
                        name: "FK_ChatRoomReceivedMessages_ChatMessages_ChatMessageId",
                        column: x => x.ChatMessageId,
                        principalTable: "ChatMessages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatRoomReceivedMessages_ChatRooms_ChatRoomId",
                        column: x => x.ChatRoomId,
                        principalTable: "ChatRooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoomReceivedMessages_ChatMessageId",
                table: "ChatRoomReceivedMessages",
                column: "ChatMessageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatRoomReceivedMessages");
        }
    }
}
