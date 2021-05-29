using Core.Const;
using DAL_EF.Entity;
using DAL_EF.Entity.Transaction;
using Microsoft.EntityFrameworkCore;

namespace DAL_EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}

        public DbSet<User> Users { get; set; }

        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<TransactionBase> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne<UserSettings>(u => u.Settings)
                .WithOne()
                .HasForeignKey<UserSettings>(us => us.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Categories)
                .WithOne()
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<User>()
                .HasMany(us => us.Wallets)
                .WithOne()
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Wallet>()
                .Property(w => w.DefaultReportPeriod)
                .IsRequired(true)
                .HasDefaultValue(ReportPeriods.CurrentMonth);

            modelBuilder.Entity<Wallet>()
                .HasMany(w => w.WalletAllowedUsers)
                .WithOne()
                .HasForeignKey(wu => wu.WalletId);

            modelBuilder.Entity<WalletAllowedUser>()
                .HasKey(wu => new { wu.WalletId, wu.UserId });

            modelBuilder.Entity<WalletAllowedUser>()
                .HasOne(wu => wu.User)
                .WithMany()
                .HasForeignKey(wu => wu.UserId);

            modelBuilder.Entity<TransactionBase>()
                .HasOne(t => t.Wallet)
                .WithMany()
                .HasForeignKey(t => t.WalletId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CategoryTransaction>()
                .HasOne(t => t.Category)
                .WithMany()
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WalletTransaction>()
                .HasOne(t => t.OtherWallet)
                .WithMany()
                .HasForeignKey(t => t.OtherWalletId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
