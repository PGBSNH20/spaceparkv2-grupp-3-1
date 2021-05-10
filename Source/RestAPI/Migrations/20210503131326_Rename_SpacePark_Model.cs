using Microsoft.EntityFrameworkCore.Migrations;

namespace RestAPI.Migrations
{
    public partial class Rename_SpacePark_Model : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parkings_SpaceParks_SpacePortId",
                table: "Parkings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpaceParks",
                table: "SpaceParks");

            migrationBuilder.RenameTable(
                name: "SpaceParks",
                newName: "SpacePorts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpacePorts",
                table: "SpacePorts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parkings_SpacePorts_SpacePortId",
                table: "Parkings",
                column: "SpacePortId",
                principalTable: "SpacePorts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parkings_SpacePorts_SpacePortId",
                table: "Parkings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SpacePorts",
                table: "SpacePorts");

            migrationBuilder.RenameTable(
                name: "SpacePorts",
                newName: "SpaceParks");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SpaceParks",
                table: "SpaceParks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Parkings_SpaceParks_SpacePortId",
                table: "Parkings",
                column: "SpacePortId",
                principalTable: "SpaceParks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
