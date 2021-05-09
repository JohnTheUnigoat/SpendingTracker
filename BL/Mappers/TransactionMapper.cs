using BL.Model.Transaction;
using DAL_EF.Entity.Transaction;

namespace BL.Mappers
{
    public static class TransactionMapper
    {
        public static CategoryTransaction ToEntity(this AddCategoryTransactionDto dto) => new CategoryTransaction
        {
            Amount = dto.Amount,
            SourceWalletId = dto.WalletId,
            CategoryId = dto.CaterodyId
        };

        public static WalletTransaction ToEntity(this AddWalletTransactionDto dto) => new WalletTransaction
        {
            Amount = dto.Amount,
            SourceWalletId = dto.WalletId,
            TargetWalletId = dto.TargetWalletId
        };
    }
}
