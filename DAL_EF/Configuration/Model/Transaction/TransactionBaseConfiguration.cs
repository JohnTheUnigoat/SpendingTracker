using DAL_EF.Entity.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL_EF.Configuration.Model.Transaction
{
    public class TransactionBaseConfiguration : IEntityTypeConfiguration<TransactionBase>
    {
        public void Configure(EntityTypeBuilder<TransactionBase> builder)
        {
            builder
                .HasOne(t => t.Wallet)
                .WithMany()
                .HasForeignKey(t => t.WalletId);
        }
    }
}