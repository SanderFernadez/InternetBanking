
using Azure;
using InternetBanking.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Infrastructure.Persistence.Contexts
{
    public class PersistenceContext: DbContext
    {

        public PersistenceContext(DbContextOptions<PersistenceContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Advance> Advances { get; set; }

        public DbSet<Beneficiary> Beneficiaries { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Transfer> Transfers { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region "Tables"

            modelBuilder.Entity<Account>().ToTable("Accounts");
            modelBuilder.Entity<Advance>().ToTable("Advances");
            modelBuilder.Entity<Beneficiary>().ToTable("Beneficiaries");

            modelBuilder.Entity<Transaction>().ToTable("Transactions");
            modelBuilder.Entity<Payment>().ToTable("Payments");
            modelBuilder.Entity<Transfer>().ToTable("Transfers");


            #endregion

            #region "Primary Keys"


            modelBuilder.Entity<Account>().HasKey(a => a.Id);
            modelBuilder.Entity<Advance>().HasKey(ad => ad.Id);
            modelBuilder.Entity<Beneficiary>().HasKey(b => b.Id); 
            modelBuilder.Entity<Transaction>().HasKey(t => t.Id);
            modelBuilder.Entity<Payment>().HasKey(p => p.Id);
            modelBuilder.Entity<Transfer>().HasKey(t => t.Id);


            #endregion

            #region "RelationShips"
            // Relationship: Account - Transaction (1:N)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Transactions)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Transaction - Payment (1:1)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Transaction)
                .WithOne(t => t.Payment)
                .HasForeignKey<Payment>(p => p.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship: Account (Credit) - CashAdvance (1:N)
            modelBuilder.Entity<Advance>()
                .HasOne(ca => ca.CreditAccount)
                .WithMany(a => a.Advances)
                .HasForeignKey(ca => ca.AccountCreditId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship: Account (Destination) - CashAdvance (1:N)
            modelBuilder.Entity<Advance>()
                .HasOne(ca => ca.DestinationAccount)
                .WithMany()
                .HasForeignKey(ca => ca.DestinationAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship: Account (Origin) - Transfer (1:N)
            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.SourceAccount)
                .WithMany(a => a.OriginTransfers)
                .HasForeignKey(t => t.AccountSourceId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship: Account (Destination) - Transfer (1:N)
            modelBuilder.Entity<Transfer>()
                .HasOne(t => t.DestinationAccount)
                .WithMany(a => a.DestinationTransfers)
                .HasForeignKey(t => t.DestinationAccountId)
                .OnDelete(DeleteBehavior.Restrict);

          


            #endregion

            #region "Property Configuration"


            #region Account
            modelBuilder.Entity<Account>()
                .Property(a => a.AccountNumber)
                .IsRequired()
                .HasMaxLength(20); // Ajusta el tamaño según sea necesario

            modelBuilder.Entity<Account>()
                .Property(a => a.InitialAmount)
                .HasColumnType("decimal(18,2)"); // Ajusta la precisión y escala

            modelBuilder.Entity<Account>()
                .Property(a => a.CurrentBalance)
                .HasColumnType("decimal(18,2)"); // Ajusta la precisión y escala

            #endregion

            #region Transaction
            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)") // Ajusta la precisión y escala
                .IsRequired();

            modelBuilder.Entity<Transaction>()
                .Property(t => t.TransactionDate)
                .IsRequired();

            modelBuilder.Entity<Transaction>()
                .Property(t => t.TransactionType)
                .IsRequired(); // Asegúrate de que el enum esté correctamente manejado

            #endregion

            #region Transfer
            modelBuilder.Entity<Transfer>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)") // Ajusta la precisión y escala
                .IsRequired();

            modelBuilder.Entity<Transfer>()
                .Property(t => t.TransferDate)
                .IsRequired();

            #endregion

            #region Payment
            modelBuilder.Entity<Payment>()
                .Property(p => p.AmountPaid)
                .HasColumnType("decimal(18,2)") // Ajusta la precisión y escala
                .IsRequired();

            modelBuilder.Entity<Payment>()
                .Property(p => p.TransactionId)
                .IsRequired(); // Asegúrate de que esté relacionado correctamente

            #endregion

            #endregion



            base.OnModelCreating(modelBuilder);
        }






    }
}
