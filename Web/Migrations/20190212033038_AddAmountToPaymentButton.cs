using Microsoft.EntityFrameworkCore.Migrations;

namespace Web.Migrations
{
    public partial class AddAmountToPaymentButton : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "PaymentButtons",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "PaymentButtons");
        }
    }
}
