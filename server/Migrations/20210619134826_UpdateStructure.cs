using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class UpdateStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TopicEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Topic = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HotScore = table.Column<int>(type: "int", nullable: false),
                    FetchedDataContextId = table.Column<int>(type: "int", nullable: false),
                    ChoronicleRecordId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TopicEntries_ChoronicleRecords_ChoronicleRecordId",
                        column: x => x.ChoronicleRecordId,
                        principalTable: "ChoronicleRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TopicEntries_ChoronicleRecordId",
                table: "TopicEntries",
                column: "ChoronicleRecordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TopicEntries");
        }
    }
}
