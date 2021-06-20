using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class FixField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicEntries_ChoronicleRecords_ChoronicleRecordId",
                table: "TopicEntries");

            migrationBuilder.DropColumn(
                name: "FetchedDataContextId",
                table: "TopicEntries");

            migrationBuilder.AlterColumn<int>(
                name: "ChoronicleRecordId",
                table: "TopicEntries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicEntries_ChoronicleRecords_ChoronicleRecordId",
                table: "TopicEntries",
                column: "ChoronicleRecordId",
                principalTable: "ChoronicleRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TopicEntries_ChoronicleRecords_ChoronicleRecordId",
                table: "TopicEntries");

            migrationBuilder.AlterColumn<int>(
                name: "ChoronicleRecordId",
                table: "TopicEntries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "FetchedDataContextId",
                table: "TopicEntries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicEntries_ChoronicleRecords_ChoronicleRecordId",
                table: "TopicEntries",
                column: "ChoronicleRecordId",
                principalTable: "ChoronicleRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
