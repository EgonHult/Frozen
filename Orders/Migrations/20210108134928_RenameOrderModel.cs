using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Orders.Migrations
{
    public partial class RenameOrderModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProduct_Order_OrderId",
                table: "OrderProduct");

            migrationBuilder.DropIndex(
                name: "IX_OrderProduct_OrderId",
                table: "OrderProduct");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderModelId",
                table: "OrderProduct",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_OrderModelId",
                table: "OrderProduct",
                column: "OrderModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProduct_Order_OrderModelId",
                table: "OrderProduct",
                column: "OrderModelId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderProduct_Order_OrderModelId",
                table: "OrderProduct");

            migrationBuilder.DropIndex(
                name: "IX_OrderProduct_OrderModelId",
                table: "OrderProduct");

            migrationBuilder.DropColumn(
                name: "OrderModelId",
                table: "OrderProduct");

            migrationBuilder.CreateIndex(
                name: "IX_OrderProduct_OrderId",
                table: "OrderProduct",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderProduct_Order_OrderId",
                table: "OrderProduct",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
