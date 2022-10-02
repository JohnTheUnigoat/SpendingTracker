using DAL_EF.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL_EF.Configuration.Model
{
    public class WallerAllowedUserConfiguration : IEntityTypeConfiguration<WalletAllowedUser>
    {
        public void Configure(EntityTypeBuilder<WalletAllowedUser> builder)
        {
            builder
                .HasKey(wu => new { wu.WalletId, wu.UserId });

            builder
                .HasOne(wu => wu.User)
                .WithMany()
                .HasForeignKey(wu => wu.UserId);
        }
    }
}