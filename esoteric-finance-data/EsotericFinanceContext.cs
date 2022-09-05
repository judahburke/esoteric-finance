using Esoteric.Finance.Abstractions.Entities.Dbo;
using Esoteric.Finance.Abstractions.Entities.Payment;
using Esoteric.Finance.Abstractions.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.DataEncryption;
using Microsoft.Extensions.Options;

namespace Esoteric.Finance.Data
{
    public class EsotericFinanceContext : DbContext
    {
        #region public
        public virtual DbSet<Category> PaymentCategories { get; set; }
        public virtual DbSet<SubCategory> PaymentSubCategories { get; set; }
        public virtual DbSet<Method> PaymentMethods { get; set; }
        public virtual DbSet<Recipient> PaymentTargets { get; set; }
        public virtual DbSet<Transaction> PaymentTransactions { get; set; }
        public virtual DbSet<TransactionSubCategory> PaymentTransactionSubCategories { get; set; }
        public virtual DbSet<TransactionMethod> PaymentTransactionMethods { get; set; }
        public virtual DbSet<GeneralLog> GeneralLogs { get; set; }

        public EsotericFinanceContext(
            DbContextOptions<EsotericFinanceContext> options,
            IOptions<AppSettings> settings,
            IEncryptionProvider encryptionProvider)
            : base(options)
        {
            _encryptionProvider = encryptionProvider;
            _databasePath = settings.Value.Data.SqlLitePath;
        }
        #endregion

        #region protected
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={_databasePath}");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.UseEncryption(_encryptionProvider);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            modelBuilder.Entity<Category>().HasData(new[]
            {
                new Category { CategoryId = 1, Name = "Correction" },
                new Category { CategoryId = 2, Name = "Miscellaneous" },
                new Category { CategoryId = 3, Name = "Auto" },
                new Category { CategoryId = 4, Name = "Beauty" },
                new Category { CategoryId = 5, Name = "Bill" },
                new Category { CategoryId = 6, Name = "Charity" },
                new Category { CategoryId = 7, Name = "Education" },
                new Category { CategoryId = 8, Name = "Entertainment" },
                new Category { CategoryId = 9, Name = "Food" },
                new Category { CategoryId = 10, Name = "Gift" },
                new Category { CategoryId = 11, Name = "Health" },
                new Category { CategoryId = 12, Name = "Home" },
                new Category { CategoryId = 13, Name = "Income" },
                new Category { CategoryId = 14, Name = "Legal" },
                new Category { CategoryId = 15, Name = "Office" },
                new Category { CategoryId = 16, Name = "Pet" },
                new Category { CategoryId = 17, Name = "Shipping" },
                new Category { CategoryId = 18, Name = "Transfer" },
                new Category { CategoryId = 19, Name = "Travel" },
                new Category { CategoryId = 20, Name = "Utilities" },
            });

            modelBuilder.Entity<SubCategory>().HasData(new[]
            {
                new SubCategory { CategoryId = 1,  SubCategoryId = 1, Name = "Correction" },
                new SubCategory { CategoryId = 2,  SubCategoryId = 2, Name = "Miscellaneous" },
                new SubCategory { CategoryId = 3,  SubCategoryId = 3, Name = "Tires" },
                new SubCategory { CategoryId = 4,  SubCategoryId = 4, Name = "Clothes" },
                new SubCategory { CategoryId = 5,  SubCategoryId = 5, Name = "Insurance" },
                new SubCategory { CategoryId = 6,  SubCategoryId = 6, Name = "Tithe" },
                new SubCategory { CategoryId = 7,  SubCategoryId = 7, Name = "Certificate" },
                new SubCategory { CategoryId = 8,  SubCategoryId = 8, Name = "Game" },
                new SubCategory { CategoryId = 9,  SubCategoryId = 9, Name = "Dine" },
                new SubCategory { CategoryId = 9,  SubCategoryId = 10, Name = "Takeout" },
                new SubCategory { CategoryId = 9,  SubCategoryId = 11, Name = "Drink" },
                new SubCategory { CategoryId = 9,  SubCategoryId = 12, Name = "Snack" },
                new SubCategory { CategoryId = 10, SubCategoryId = 13, Name = "Birthday, Mom" },
                new SubCategory { CategoryId = 11, SubCategoryId = 14, Name = "Medicine" },
                new SubCategory { CategoryId = 12, SubCategoryId = 15, Name = "Cleaning Supplies" },
                new SubCategory { CategoryId = 13, SubCategoryId = 16, Name = "Paycheck" },
                new SubCategory { CategoryId = 14, SubCategoryId = 17, Name = "Taxes" },
                new SubCategory { CategoryId = 15, SubCategoryId = 18, Name = "Paper" },
                new SubCategory { CategoryId = 16, SubCategoryId = 19, Name = "Litter" },
                new SubCategory { CategoryId = 17, SubCategoryId = 20, Name = "Stamps" },
                new SubCategory { CategoryId = 18, SubCategoryId = 21, Name = "Withdrawal" },
                new SubCategory { CategoryId = 18, SubCategoryId = 22, Name = "Deposit" },
                new SubCategory { CategoryId = 18, SubCategoryId = 23, Name = "Pay Credit Card" },
                new SubCategory { CategoryId = 19, SubCategoryId = 24, Name = "Gas" },
                new SubCategory { CategoryId = 20, SubCategoryId = 25, Name = "Internet" },
            });

            modelBuilder.Entity<Method>().HasData(new[]
            {
                new Method { MethodId = 1, Name = "Cash" },
            });

            modelBuilder.Entity<Recipient>().HasData(new[]
            {
                new Recipient { RecipientId = 1, Name = "IRS" },
                new Recipient { RecipientId = 2, Name = "Amazon" },
                new Recipient { RecipientId = 3, Name = "Walmart" },
                new Recipient { RecipientId = 4, Name = "Target" },
            });
        }
        #endregion

        #region private
        private readonly IEncryptionProvider _encryptionProvider;
        private readonly string _databasePath;
        #endregion
    }
}