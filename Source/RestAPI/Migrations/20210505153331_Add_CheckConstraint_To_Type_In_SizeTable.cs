using Microsoft.EntityFrameworkCore.Migrations;

namespace RestAPI.Migrations
{
    public partial class Add_CheckConstraint_To_Type_In_SizeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddCheckConstraint(
                name: "CK_Only_Range_Between_1_and_4",
                table: "Sizes",
                sql: "[Type] >= 1 AND [Type] < 5");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Only_Range_Between_1_and_4",
                table: "Sizes");
        }
    }
}
