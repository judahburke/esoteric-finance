using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Esoteric.Finance.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Payment");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Category",
                schema: "Payment",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "current_timestamp"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, defaultValue: "esoteric-finance-api"),
                    Name = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "GeneralLog",
                schema: "dbo",
                columns: table => new
                {
                    GeneralLogId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventCode = table.Column<int>(type: "INTEGER", nullable: false),
                    LevelCode = table.Column<int>(type: "INTEGER", nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Scope = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Message = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    Exception = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "current_timestamp"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, defaultValue: "esoteric-finance-api")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralLog", x => x.GeneralLogId);
                });

            migrationBuilder.CreateTable(
                name: "Initiator",
                schema: "Payment",
                columns: table => new
                {
                    InitiatorId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "current_timestamp"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, defaultValue: "esoteric-finance-api"),
                    Name = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiator", x => x.InitiatorId);
                });

            migrationBuilder.CreateTable(
                name: "Method",
                schema: "Payment",
                columns: table => new
                {
                    MethodId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "current_timestamp"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, defaultValue: "esoteric-finance-api"),
                    Name = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Method", x => x.MethodId);
                });

            migrationBuilder.CreateTable(
                name: "Recipient",
                schema: "Payment",
                columns: table => new
                {
                    RecipientId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "current_timestamp"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, defaultValue: "esoteric-finance-api"),
                    Name = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipient", x => x.RecipientId);
                });

            migrationBuilder.CreateTable(
                name: "Detail",
                schema: "Payment",
                columns: table => new
                {
                    DetailId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "current_timestamp"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, defaultValue: "esoteric-finance-api")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detail", x => x.DetailId);
                    table.ForeignKey(
                        name: "FK_Detail_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Payment",
                        principalTable: "Category",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                schema: "Payment",
                columns: table => new
                {
                    TransactionId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InitiatorId = table.Column<int>(type: "INTEGER", nullable: false),
                    RecipientId = table.Column<int>(type: "INTEGER", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PostedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "current_timestamp"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, defaultValue: "esoteric-finance-api")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transaction_Initiator_InitiatorId",
                        column: x => x.InitiatorId,
                        principalSchema: "Payment",
                        principalTable: "Initiator",
                        principalColumn: "InitiatorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transaction_Recipient_RecipientId",
                        column: x => x.RecipientId,
                        principalSchema: "Payment",
                        principalTable: "Recipient",
                        principalColumn: "RecipientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionDetail",
                schema: "Payment",
                columns: table => new
                {
                    TransactionDetailId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransactionId = table.Column<long>(type: "INTEGER", nullable: false),
                    DetailId = table.Column<long>(type: "INTEGER", nullable: false),
                    Multiplier = table.Column<float>(type: "REAL", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "current_timestamp"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, defaultValue: "esoteric-finance-api")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionDetail", x => x.TransactionDetailId);
                    table.ForeignKey(
                        name: "FK_TransactionDetail_Detail_DetailId",
                        column: x => x.DetailId,
                        principalSchema: "Payment",
                        principalTable: "Detail",
                        principalColumn: "DetailId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionDetail_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalSchema: "Payment",
                        principalTable: "Transaction",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionMethod",
                schema: "Payment",
                columns: table => new
                {
                    TransactionMethodId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransactionId = table.Column<long>(type: "INTEGER", nullable: false),
                    MethodId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "current_timestamp"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, defaultValue: "esoteric-finance-api")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionMethod", x => x.TransactionMethodId);
                    table.ForeignKey(
                        name: "FK_TransactionMethod_Method_MethodId",
                        column: x => x.MethodId,
                        principalSchema: "Payment",
                        principalTable: "Method",
                        principalColumn: "MethodId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionMethod_Transaction_TransactionId",
                        column: x => x.TransactionId,
                        principalSchema: "Payment",
                        principalTable: "Transaction",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 1, "Correction" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 2, "Miscellaneous" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 3, "Auto" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 4, "Beauty" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 5, "Bill" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 6, "Charity" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 7, "Education" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 8, "Entertainment" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 9, "Food" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 10, "Gift" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 11, "Health" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 12, "Home" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 13, "Income" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 14, "Legal" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 15, "Office" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 16, "Pet" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 17, "Shipping" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 18, "Transfer" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 19, "Travel" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Category",
                columns: new[] { "CategoryId", "Name" },
                values: new object[] { 20, "Utilities" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Initiator",
                columns: new[] { "InitiatorId", "Name" },
                values: new object[] { 1, "Account Owner" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Method",
                columns: new[] { "MethodId", "Name" },
                values: new object[] { 1, "Cash" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Recipient",
                columns: new[] { "RecipientId", "Name" },
                values: new object[] { 1, "IRS" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Recipient",
                columns: new[] { "RecipientId", "Name" },
                values: new object[] { 2, "Amazon" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Recipient",
                columns: new[] { "RecipientId", "Name" },
                values: new object[] { 3, "Walmart" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Recipient",
                columns: new[] { "RecipientId", "Name" },
                values: new object[] { 4, "Target" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 1L, 1, "Correction" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 2L, 2, "Miscellaneous" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 3L, 3, "Tires" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 4L, 4, "Clothes" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 5L, 5, "Insurance" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 6L, 6, "Tithe" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 7L, 7, "Certificate" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 8L, 8, "Game" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 9L, 9, "Dine" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 10L, 9, "Takeout" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 11L, 9, "Drink" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 12L, 9, "Snack" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 13L, 10, "Birthday, Mom" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 14L, 11, "Medicine" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 15L, 12, "Cleaning Supplies" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 16L, 13, "Paycheck" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 17L, 14, "Taxes" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 18L, 15, "Paper" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 19L, 16, "Litter" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 20L, 17, "Stamps" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 21L, 18, "Withdrawal" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 22L, 18, "Deposit" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 23L, 18, "Pay Credit Card" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 24L, 19, "Gas" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "Detail",
                columns: new[] { "DetailId", "CategoryId", "Description" },
                values: new object[] { 25L, 20, "Internet" });

            migrationBuilder.CreateIndex(
                name: "IX_Detail_CategoryId",
                schema: "Payment",
                table: "Detail",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_InitiatorId",
                schema: "Payment",
                table: "Transaction",
                column: "InitiatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_RecipientId",
                schema: "Payment",
                table: "Transaction",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetail_DetailId",
                schema: "Payment",
                table: "TransactionDetail",
                column: "DetailId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionDetail_TransactionId",
                schema: "Payment",
                table: "TransactionDetail",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionMethod_MethodId",
                schema: "Payment",
                table: "TransactionMethod",
                column: "MethodId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionMethod_TransactionId",
                schema: "Payment",
                table: "TransactionMethod",
                column: "TransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralLog",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TransactionDetail",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "TransactionMethod",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "Detail",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "Method",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "Transaction",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "Initiator",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "Recipient",
                schema: "Payment");
        }
    }
}
