using DAL_EF.Entity.Transaction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL_EF.Configuration.Model.Transaction
{
    public class CategoryTransactionConfiguration : IEntityTypeConfiguration<CategoryTransaction>
    {
        public void Configure(EntityTypeBuilder<CategoryTransaction> builder)
        {
            builder
                .HasOne(t => t.Category)
                .WithMany()
                .HasForeignKey(t => t.CategoryId);
        }
    }
}