using DAL_EF.Entity.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL_EF.Configuration.Model.Transaction
{
    public class WalletTransactionConfiguration : IEntityTypeConfiguration<WalletTransaction>
    {
        public void Configure(EntityTypeBuilder<WalletTransaction> builder)
        {
            builder
                .HasOne(t => t.OtherWallet)
                .WithMany()
                .HasForeignKey(t => t.OtherWalletId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}