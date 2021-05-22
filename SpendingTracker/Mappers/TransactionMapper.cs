using BL.Model.Transaction;
using SpendingTracker.Models.Transaction.Request;
using SpendingTracker.Models.Transaction.Response;
using System.Collections.Generic;
using System.Linq;

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

        public static GetTransactionsDto ToDto(this GetTransactionRequest request, int walletId) => new GetTransactionsDto
        {
            WalletId = walletId,
            ReportPeriod = request.ReportPeriod,
            CustomFromDate = request.From,
            CustomToDate = request.To
        };

        public static TransactionResponse ToResponse(this ShortTransactionDomain domain) => new TransactionResponse
        {
            Id = domain.Id,
            Target = domain.Target,
            Amount = domain.Amount,
            Timestamp = domain.Timestamp
        };

        public static List<TransactionResponse> AllToResponse(this IEnumerable<ShortTransactionDomain> domains) =>
            domains.Select(d => d.ToResponse()).ToList();
    }
}
