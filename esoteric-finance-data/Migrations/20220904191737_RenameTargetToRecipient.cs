using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Esoteric.Finance.Data.Migrations
{
    public partial class RenameTargetToRecipient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Recipient_TargetId",
                schema: "Payment",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "TargetId",
                schema: "Payment",
                table: "Transaction",
                newName: "RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_TargetId",
                schema: "Payment",
                table: "Transaction",
                newName: "IX_Transaction_RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Recipient_RecipientId",
                schema: "Payment",
                table: "Transaction",
                column: "RecipientId",
                principalSchema: "Payment",
                principalTable: "Recipient",
                principalColumn: "RecipientId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Recipient_RecipientId",
                schema: "Payment",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "RecipientId",
                schema: "Payment",
                table: "Transaction",
                newName: "TargetId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_RecipientId",
                schema: "Payment",
                table: "Transaction",
                newName: "IX_Transaction_TargetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Recipient_TargetId",
                schema: "Payment",
                table: "Transaction",
                column: "TargetId",
                principalSchema: "Payment",
                principalTable: "Recipient",
                principalColumn: "RecipientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
