using BL.Model.Transaction;
using DAL_EF.Entity.Transaction;

namespace BL.Mappers
{
    public static class TransactionMapper
    {
        public static CategoryTransaction ToEntity(this AddCategoryTransactionDto dto) => new CategoryTransaction
        {
            Amount = dto.Amount,
            WalletId = dto.WalletId,
            CategoryId = dto.CaterodyId
        };

        public static WalletTransaction ToEntity(this AddWalletTransactionDto dto) => new WalletTransaction
        {
            Amount = dto.Amount,
            WalletId = dto.WalletId,
            TargetWalletId = dto.TargetWalletId
        };
    }
}
