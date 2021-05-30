using BL.Model.Transaction;
using SpendingTracker.Models.Transaction.Request;
using SpendingTracker.Models.Transaction.Response;
using System.Collections.Generic;
using System.Linq;

namespace SpendingTracker.Mappers
{
    public static class TransactionMapper
    {
        public static AddUpdateCategoryTransactionDto ToCategoryDto(
            this AddUpdateTransactionRequest request,
            int walletId,
            int userId) => new()
            {
                WalletId = walletId,
                Amount = request.Amount,
                ManualTimestamp = request.ManualTimestamp,
                UserId = userId,
                CaterodyId = request.CategoryId.Value
            };

        public static AddUpdateWalletTransactionDto ToWalletDto(
            this AddUpdateTransactionRequest request,
            int walletId,
            int userId) => new()
            {
                WalletId = walletId,
                Amount = request.Amount,
                ManualTimestamp = request.ManualTimestamp,
                UserId = userId,
                OtherWalletId = request.OtherWalletId.Value
            };

        public static GetTransactionsDto ToDto(this GetTransactionRequest request, int walletId) => new GetTransactionsDto
        {
            WalletId = walletId,
            ReportPeriod = request.ReportPeriod,
            CustomFromDate = request.From,
            CustomToDate = request.To
        };

        public static TransactionResponse ToResponse(this TransactionDomain domain) => new TransactionResponse
        {
            Id = domain.Id,
            CategoryId = domain.CategoryId,
            OtherWalletId = domain.OtherWalletId,
            TargetLabel = domain.TargetLabel,
            Amount = domain.Amount,
            Timestamp = domain.Timestamp
        };

        public static List<TransactionResponse> AllToResponse(this IEnumerable<TransactionDomain> domains) =>
            domains.Select(d => d.ToResponse()).ToList();


        public static ShortTransactionSummaryResponse ToResponse(
            this ShortTransactionSummaryDomain domain) => new ShortTransactionSummaryResponse
            {
                Income = domain.Income,
                Expense = domain.Expense
            };

        public static CategoryOrWalletSummaryResponse ToResponse(
            this CategoryOrWalletSummaryDomain domain) => new CategoryOrWalletSummaryResponse
            {
                Id = domain.Id,
                Name = domain.Name,
                Amount = domain.Amount
            };

        public static IEnumerable<CategoryOrWalletSummaryResponse> AllToResponse(
            this IEnumerable<CategoryOrWalletSummaryDomain> domains) => domains
            .Select(d => d.ToResponse())
            .ToList();

        public static OneWaySummaryResponse ToResponse(
            this OneWaySummaryDomain domain) => new OneWaySummaryResponse
            {
                Categories = domain.Categories.AllToResponse(),
                Wallets = domain.Wallets.AllToResponse()
            };

        public static TransactionSummaryResponse ToResponse(
            this TransactionSummaryDomain domain) => new TransactionSummaryResponse
            {
                TotalIncome = domain.TotalIncome,
                TotalExpense = domain.TotalExpense,
                IncomeDetails = domain.IncomeDetails.ToResponse(),
                ExpenseDetails = domain.ExpenseDetails.ToResponse()
            };
    }
}
