using Core.Const;
using DAL_EF.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL_EF.Configuration.Model
{
    public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder
                .Property(w => w.DefaultReportPeriod)
                .IsRequired()
                .HasDefaultValue(ReportPeriods.CurrentMonth);

            builder
                .HasMany(w => w.Categories)
                .WithOne()
                .HasForeignKey(c => c.WalletId);
            
            builder
                .HasMany(w => w.WalletAllowedUsers)
                .WithOne()
                .HasForeignKey(wu => wu.WalletId);
        }
    }
}