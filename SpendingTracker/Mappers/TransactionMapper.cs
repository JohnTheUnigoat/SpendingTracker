using BL.Model.Transaction;
using Core.Exceptions;
using SpendingTracker.Models.Transaction.Request;

namespace SpendingTracker.Mappers
{
    public static class TransactionMapper
    {
        public static AddTransactionDtoBase ToDto(this AddTransactionRequest request, int walletId)
        {
            if (request.TargetWalletId.HasValue && request.CategoryId.HasValue)
            {
                throw new ValidationException(new()
                {
                    { nameof(request.TargetWalletId), $"Only one of the fields {nameof(request.TargetWalletId)}, {nameof(request.CategoryId)} should have a value." },
                    { nameof(request.CategoryId), $"Only one of the fields {nameof(request.TargetWalletId)}, {nameof(request.CategoryId)} should have a value." },
                });
            }

            if (request.CategoryId.HasValue)
                return request.ToCategoryDto(walletId);
            else
                return request.ToWalletDto(walletId);
        }

        private static AddCategoryTransactionDto ToCategoryDto(
            this AddTransactionRequest request,
            int walletId) => new()
            {
                WalletId = walletId,
                Amount = request.Amount,
                CaterodyId = request.CategoryId.Value
            };

        private static AddWalletTransactionDto ToWalletDto(
            this AddTransactionRequest request,
            int walletId) => new()
            {
                WalletId = walletId,
                Amount = request.Amount,
                TargetWalletId = request.TargetWalletId.Value
            };
    }
}
