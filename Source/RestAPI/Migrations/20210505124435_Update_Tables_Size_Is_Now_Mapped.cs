using Microsoft.EntityFrameworkCore.Migrations;

namespace RestAPI.Migrations
{
    public partial class Update_Tables_Size_Is_Now_Mapped : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Receipts_SizeId",
                table: "Receipts",
                column: "SizeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_Sizes_SizeId",
                table: "Receipts",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_Sizes_SizeId",
                table: "Receipts");

            migrationBuilder.DropIndex(
                name: "IX_Receipts_SizeId",
                table: "Receipts");
        }
    }
}
