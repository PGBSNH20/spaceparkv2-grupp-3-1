using Microsoft.EntityFrameworkCore.Migrations;

namespace RestAPI.Migrations
{
    public partial class Rename_Models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parkings_SpaceParks_SpaceParkId",
                table: "Parkings");

            migrationBuilder.RenameColumn(
                name: "SpaceParkId",
                table: "Parkings",
                newName: "SpacePortId");

            migrationBuilder.RenameIndex(
                name: "IX_Parkings_SpaceParkId",
                table: "Parkings",
                newName: "IX_Parkings_SpacePortId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parkings_SpaceParks_SpacePortId",
                table: "Parkings",
                column: "SpacePortId",
                principalTable: "SpaceParks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parkings_SpaceParks_SpacePortId",
                table: "Parkings");

            migrationBuilder.RenameColumn(
                name: "SpacePortId",
                table: "Parkings",
                newName: "SpaceParkId");

            migrationBuilder.RenameIndex(
                name: "IX_Parkings_SpacePortId",
                table: "Parkings",
                newName: "IX_Parkings_SpaceParkId");

            migrationBuilder.AddForeignKey(
                name: "FK_Parkings_SpaceParks_SpaceParkId",
                table: "Parkings",
                column: "SpaceParkId",
                principalTable: "SpaceParks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
