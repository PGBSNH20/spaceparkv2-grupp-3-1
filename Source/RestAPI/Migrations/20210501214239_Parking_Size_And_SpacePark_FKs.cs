using Microsoft.EntityFrameworkCore.Migrations;

namespace RestAPI.Migrations
{
    public partial class Parking_Size_And_SpacePark_FKs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parkings_Sizes_SizeId",
                table: "Parkings");

            migrationBuilder.DropForeignKey(
                name: "FK_Parkings_SpaceParks_SpaceParkId",
                table: "Parkings");

            migrationBuilder.AlterColumn<int>(
                name: "SpaceParkId",
                table: "Parkings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SizeId",
                table: "Parkings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Parkings_Sizes_SizeId",
                table: "Parkings",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Parkings_SpaceParks_SpaceParkId",
                table: "Parkings",
                column: "SpaceParkId",
                principalTable: "SpaceParks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Parkings_Sizes_SizeId",
                table: "Parkings");

            migrationBuilder.DropForeignKey(
                name: "FK_Parkings_SpaceParks_SpaceParkId",
                table: "Parkings");

            migrationBuilder.AlterColumn<int>(
                name: "SpaceParkId",
                table: "Parkings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SizeId",
                table: "Parkings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Parkings_Sizes_SizeId",
                table: "Parkings",
                column: "SizeId",
                principalTable: "Sizes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Parkings_SpaceParks_SpaceParkId",
                table: "Parkings",
                column: "SpaceParkId",
                principalTable: "SpaceParks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
