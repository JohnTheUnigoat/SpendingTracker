using BL.Mappers;
using BL.Model.Transaction;
using Core.Const;
using Core.Exceptions;
using Core.Exceptions.CustomExceptions;
using DAL_EF;
using DAL_EF.Entity;
using DAL_EF.Entity.Transaction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Impl
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _dbContext;

        public TransactionService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private (DateTime from, DateTime to) GetReportPeriodLimits(string reportPeriod)
        {
            DateTime currDate = DateTime.Today;

            DateTime fromDate;
            DateTime toDate;

            switch (reportPeriod)
            {
                case ReportPeriods.CurrentDay:
                    fromDate = currDate;
                    toDate = fromDate.AddHours(24);
                    break;
                case ReportPeriods.CurrentWeek:
                    var weekStart = DayOfWeek.Monday;

                    fromDate = currDate.AddDays(-(int)currDate.DayOfWeek + (int)weekStart);
                    toDate = fromDate.AddDays(7);
                    break;
                case ReportPeriods.CurrentMonth:
                    fromDate = currDate.AddDays(-currDate.Day + 1);
                    toDate = fromDate.AddMonths(1);
                    break;
                case ReportPeriods.CurrentYear:
                    fromDate = new DateTime(currDate.Year, 1, 1);
                    toDate = fromDate.AddYears(1);
                    break;
                case ReportPeriods.AllTime:
                    fromDate = new DateTime(0);
                    toDate = DateTime.Now;
                    break;
                case ReportPeriods.Custom: throw new ArgumentException("A custom report period can't be automatically determined.");
                default: throw new ArgumentException($"reportPeriod value should be one of: {string.Join(", ", ReportPeriods.GetValues())}.");
            }

            return (fromDate, toDate);
        }

        public async Task<List<TransactionDomain>> GetTransactionsAsync(GetTransactionsDto dto)
        {
            Wallet wallet = _dbContext.Wallets.Find(dto.WalletId);

            if (wallet == null)
            {
                throw new HttpStatusException(404);
            }

            string reportPeriod = string.IsNullOrEmpty(dto.ReportPeriod) ?
                wallet.DefaultReportPeriod :
                dto.ReportPeriod;

            DateTime fromDate, toDate;

            if (reportPeriod == ReportPeriods.Custom)
            {
                fromDate = dto.CustomFromDate.Value;
                toDate = dto.CustomToDate.Value;
            }
            else
            {
                try
                {
                    (fromDate, toDate) = GetReportPeriodLimits(reportPeriod);
                }
                catch (ArgumentException e)
                {
                    throw new ValidationException(new() { { nameof(dto.ReportPeriod), e.Message } });
                }
            }

            IQueryable<TransactionDomain> categoryTransactions = _dbContext.Transactions
                .Where(t =>
                    t is CategoryTransaction &&
                    t.WalletId == dto.WalletId)
                .Select(t => new TransactionDomain
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    CategoryId = (t as CategoryTransaction).CategoryId,
                    OtherWalletId = null,
                    TargetLabel = (t as CategoryTransaction).Category.Name,
                    Timestamp = t.TimeStamp
                });

            IQueryable<TransactionDomain> outgoingWalletTransaction = _dbContext.Transactions
                .Where(t =>
                    t is WalletTransaction &&
                    t.WalletId == dto.WalletId)
                .Select(t => new TransactionDomain
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    CategoryId = null,
                    OtherWalletId = (t as WalletTransaction).OtherWalletId,
                    TargetLabel = (t as WalletTransaction).OtherWallet.Name,
                    Timestamp = t.TimeStamp
                });

            IQueryable<TransactionDomain> incomingWalletTransaction = _dbContext.Transactions
                .Where(t =>
                    t is WalletTransaction &&
                    (t as WalletTransaction).OtherWalletId == dto.WalletId)
                .Select(t => new TransactionDomain
                {
                    Id = t.Id,
                    Amount = t.Amount * -1,
                    CategoryId = null,
                    OtherWalletId = t.WalletId,
                    TargetLabel = t.Wallet.Name,
                    Timestamp = t.TimeStamp
                });

            List<TransactionDomain> res = await
                categoryTransactions
                    .Union(outgoingWalletTransaction)
                    .Union(incomingWalletTransaction)
                    .Where(t => t.Timestamp >= fromDate && t.Timestamp < toDate)
                    .OrderBy(t => t.Timestamp)
                .ToListAsync();

            return res;
        }

        private void ValidateTransactionDto(AddUpdateTransactionDtoBase dto)
        {
            if (dto.Amount == 0)
            {
                throw new ValidationException(new()
                {
                    { nameof(dto.Amount), "Transaction amount can't be 0." }
                });
            }

            if (dto is AddUpdateWalletTransactionDto)
            {
                var wDto = dto as AddUpdateWalletTransactionDto;

                if (wDto.WalletId == wDto.OtherWalletId)
                {
                    throw new ValidationException(new()
                    {
                        { nameof(wDto.OtherWalletId), "Source wallet can not be the same as the wallet transaction belongs to." }
                    });
                }
            }
        }

        public async Task<int> AddTransactionAsync(AddUpdateTransactionDtoBase dto)
        {
            ValidateTransactionDto(dto);

            TransactionBase newTransaction;

            switch (dto)
            {
                case AddUpdateCategoryTransactionDto categoryDto:
                    newTransaction = categoryDto.ToEntity();
                    break;
                case AddUpdateWalletTransactionDto walletDto:
                    newTransaction = walletDto.ToEntity();
                    break;
                default: throw new ArgumentException("Unknown/unhandled addTransactionDto type.");
            }

            newTransaction.TimeStamp = dto.ManualTimestamp ?? DateTime.Now;

            _dbContext.Transactions.Add(newTransaction);

            await _dbContext.SaveChangesAsync();

            return newTransaction.Id;
        }

        public async Task<TransactionDomain> UpdateTransaction(int transactionId, AddUpdateTransactionDtoBase dto)
        {
            ValidateTransactionDto(dto);

            TransactionBase transaction = _dbContext.Transactions.Find(transactionId);

            if (dto is AddUpdateCategoryTransactionDto && transaction is WalletTransaction ||
                dto is AddUpdateWalletTransactionDto && transaction is CategoryTransaction)
            {
                throw new AttemptedChangeOfTransactionTypeException();
            }

            transaction.Amount = dto.Amount;
            transaction.WalletId = dto.WalletId;

            if (dto.ManualTimestamp != null)
                transaction.TimeStamp = dto.ManualTimestamp.Value;

            switch (transaction)
            {
                case CategoryTransaction ct:
                    var cDto = dto as AddUpdateCategoryTransactionDto;
                    ct.CategoryId = cDto.CaterodyId;
                    break;
                case WalletTransaction wt:
                    var wDto = dto as AddUpdateWalletTransactionDto;
                    wt.OtherWalletId = wDto.OtherWalletId;
                    break;
                default: throw new NotImplementedException(
                    "Updating for this transaction type is not implemented (or something went wrong)");
            }

            await _dbContext.SaveChangesAsync();

            return await _dbContext.Transactions
                .Where(t => t.Id == transactionId)
                .Select(t => new TransactionDomain
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    TargetLabel = (t is CategoryTransaction) ?
                        (t as CategoryTransaction).Category.Name :
                        (t as WalletTransaction).OtherWallet.Name,
                    Timestamp = t.TimeStamp
                })
                .SingleAsync();
        }

        public async Task DeleteTransaction(int transactionId)
        {
            TransactionBase transaction = new CategoryTransaction
            {
                Id = transactionId
            };

            _dbContext.Remove(transaction);

            await _dbContext.SaveChangesAsync();
        }
    }
}
