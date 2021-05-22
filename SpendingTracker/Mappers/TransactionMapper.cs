using BL.Model.Transaction;
using SpendingTracker.Models.Transaction.Request;

namespace SpendingTracker.Mappers
{
    public static class TransactionMapper
    {
        public static AddCategoryTransactionDto ToCategoryDto(
            this AddTransactionRequest request,
            int walletId) => new()
            {
                WalletId = walletId,
                Amount = request.Amount,
                CaterodyId = request.CategoryId.Value
            };

        public static AddWalletTransactionDto ToWalletDto(
            this AddTransactionRequest request,
            int walletId) => new()
            {
                WalletId = walletId,
                Amount = request.Amount,
                TargetWalletId = request.TargetWalletId.Value
            };
    }
}
