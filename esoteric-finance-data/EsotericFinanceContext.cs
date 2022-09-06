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
        public virtual DbSet<Detail> PaymentDetails { get; set; }
        public virtual DbSet<Initiator> PaymentInitiators { get; set; }
        public virtual DbSet<Method> PaymentMethods { get; set; }
        public virtual DbSet<Recipient> PaymentRecipients { get; set; }
        public virtual DbSet<Transaction> PaymentTransactions { get; set; }
        public virtual DbSet<TransactionDetail> PaymentTransactionDetails { get; set; }
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

            modelBuilder.Entity<Detail>().HasData(new[]
            {
                new Detail { CategoryId = 1,  DetailId = 1, Description = "Correction" },
                new Detail { CategoryId = 2,  DetailId = 2, Description = "Miscellaneous" },
                new Detail { CategoryId = 3,  DetailId = 3, Description = "Tires" },
                new Detail { CategoryId = 4,  DetailId = 4, Description = "Clothes" },
                new Detail { CategoryId = 5,  DetailId = 5, Description = "Insurance" },
                new Detail { CategoryId = 6,  DetailId = 6, Description = "Tithe" },
                new Detail { CategoryId = 7,  DetailId = 7, Description = "Certificate" },
                new Detail { CategoryId = 8,  DetailId = 8, Description = "Game" },
                new Detail { CategoryId = 9,  DetailId = 9, Description = "Dine" },
                new Detail { CategoryId = 9,  DetailId = 10, Description = "Takeout" },
                new Detail { CategoryId = 9,  DetailId = 11, Description = "Drink" },
                new Detail { CategoryId = 9,  DetailId = 12, Description = "Snack" },
                new Detail { CategoryId = 10, DetailId = 13, Description = "Birthday, Mom" },
                new Detail { CategoryId = 11, DetailId = 14, Description = "Medicine" },
                new Detail { CategoryId = 12, DetailId = 15, Description = "Cleaning Supplies" },
                new Detail { CategoryId = 13, DetailId = 16, Description = "Paycheck" },
                new Detail { CategoryId = 14, DetailId = 17, Description = "Taxes" },
                new Detail { CategoryId = 15, DetailId = 18, Description = "Paper" },
                new Detail { CategoryId = 16, DetailId = 19, Description = "Litter" },
                new Detail { CategoryId = 17, DetailId = 20, Description = "Stamps" },
                new Detail { CategoryId = 18, DetailId = 21, Description = "Withdrawal" },
                new Detail { CategoryId = 18, DetailId = 22, Description = "Deposit" },
                new Detail { CategoryId = 18, DetailId = 23, Description = "Pay Credit Card" },
                new Detail { CategoryId = 19, DetailId = 24, Description = "Gas" },
                new Detail { CategoryId = 20, DetailId = 25, Description = "Internet" },
            });

            modelBuilder.Entity<Initiator>().HasData(new[]
            {
                new Initiator { InitiatorId = 1, Name = "Account Owner" },
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