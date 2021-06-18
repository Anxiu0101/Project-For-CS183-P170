using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class CorrectFieldType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChoronicleRecords_ChoronicleRecords_TypeId",
                table: "ChoronicleRecords");

            migrationBuilder.DropIndex(
                name: "IX_ChoronicleRecords_TypeId",
                table: "ChoronicleRecords");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "ChoronicleRecords");

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "ChoronicleRecords",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "ChoronicleRecords");

            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "ChoronicleRecords",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChoronicleRecords_TypeId",
                table: "ChoronicleRecords",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChoronicleRecords_ChoronicleRecords_TypeId",
                table: "ChoronicleRecords",
                column: "TypeId",
                principalTable: "ChoronicleRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
