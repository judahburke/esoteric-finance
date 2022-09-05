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
                name: "SubCategory",
                schema: "Payment",
                columns: table => new
                {
                    SubCategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "current_timestamp"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, defaultValue: "esoteric-finance-api"),
                    Name = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategory", x => x.SubCategoryId);
                    table.ForeignKey(
                        name: "FK_SubCategory_Category_CategoryId",
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
                    TargetId = table.Column<int>(type: "INTEGER", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PostedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "current_timestamp"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, defaultValue: "esoteric-finance-api")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transaction_Recipient_TargetId",
                        column: x => x.TargetId,
                        principalSchema: "Payment",
                        principalTable: "Recipient",
                        principalColumn: "RecipientId",
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
                    Amount = table.Column<double>(type: "REAL", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "TransactionSubCategory",
                schema: "Payment",
                columns: table => new
                {
                    TransactionSubCategoryId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TransactionId = table.Column<long>(type: "INTEGER", nullable: false),
                    SubCategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Multiplier = table.Column<double>(type: "REAL", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "current_timestamp"),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, defaultValue: "esoteric-finance-api")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionSubCategory", x => x.TransactionSubCategoryId);
                    table.ForeignKey(
                        name: "FK_TransactionSubCategory_SubCategory_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalSchema: "Payment",
                        principalTable: "SubCategory",
                        principalColumn: "SubCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionSubCategory_Transaction_TransactionId",
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
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 1, 1, "Correction" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 2, 2, "Miscellaneous" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 3, 3, "Tires" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 4, 4, "Clothes" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 5, 5, "Insurance" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 6, 6, "Tithe" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 7, 7, "Certificate" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 8, 8, "Game" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 9, 9, "Dine" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 10, 9, "Takeout" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 11, 9, "Drink" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 12, 9, "Snack" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 13, 10, "Birthday, Mom" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 14, 11, "Medicine" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 15, 12, "Cleaning Supplies" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 16, 13, "Paycheck" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 17, 14, "Taxes" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 18, 15, "Paper" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 19, 16, "Litter" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 20, 17, "Stamps" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 21, 18, "Withdrawal" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 22, 18, "Deposit" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 23, 18, "Pay Credit Card" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 24, 19, "Gas" });

            migrationBuilder.InsertData(
                schema: "Payment",
                table: "SubCategory",
                columns: new[] { "SubCategoryId", "CategoryId", "Name" },
                values: new object[] { 25, 20, "Internet" });

            migrationBuilder.CreateIndex(
                name: "IX_SubCategory_CategoryId",
                schema: "Payment",
                table: "SubCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_TargetId",
                schema: "Payment",
                table: "Transaction",
                column: "TargetId");

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

            migrationBuilder.CreateIndex(
                name: "IX_TransactionSubCategory_SubCategoryId",
                schema: "Payment",
                table: "TransactionSubCategory",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionSubCategory_TransactionId",
                schema: "Payment",
                table: "TransactionSubCategory",
                column: "TransactionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralLog",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "TransactionMethod",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "TransactionSubCategory",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "Method",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "SubCategory",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "Transaction",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "Category",
                schema: "Payment");

            migrationBuilder.DropTable(
                name: "Recipient",
                schema: "Payment");
        }
    }
}
