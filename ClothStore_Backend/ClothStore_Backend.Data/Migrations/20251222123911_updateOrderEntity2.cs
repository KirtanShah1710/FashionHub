using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClothStore_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateOrderEntity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentOrderId",
                table: "Orders",
                newName: "RazorpayPaymentId");

            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "Orders",
                newName: "RazorpayOrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RazorpayPaymentId",
                table: "Orders",
                newName: "PaymentOrderId");

            migrationBuilder.RenameColumn(
                name: "RazorpayOrderId",
                table: "Orders",
                newName: "PaymentId");
        }
    }
}
