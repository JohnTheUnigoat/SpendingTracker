using DAL_EF.Entity;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne<UserSettings>(u => u.Settings)
                .WithOne()
                .HasForeignKey<UserSettings>(us => us.UserId);

            modelBuilder.Entity<UserSettings>()
                .HasMany(us => us.Wallets)
                .WithOne()
                .HasForeignKey(w => w.UserId);

            modelBuilder.Entity<Wallet>()
                .HasMany(w => w.Categories)
                .WithOne()
                .HasForeignKey(c => c.WalletId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
