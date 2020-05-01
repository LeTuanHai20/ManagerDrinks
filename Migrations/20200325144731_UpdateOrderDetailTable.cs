using Microsoft.EntityFrameworkCore.Migrations;

namespace DrinkAndGo.Migrations
{
    public partial class UpdateOrderDetailTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Drinks_DrinkId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "Drinkd",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<int>(
                name: "DrinkId",
                table: "OrderDetails",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Drinks_DrinkId",
                table: "OrderDetails",
                column: "DrinkId",
                principalTable: "Drinks",
                principalColumn: "DrinkId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Drinks_DrinkId",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<int>(
                name: "DrinkId",
                table: "OrderDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Drinkd",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Drinks_DrinkId",
                table: "OrderDetails",
                column: "DrinkId",
                principalTable: "Drinks",
                principalColumn: "DrinkId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
