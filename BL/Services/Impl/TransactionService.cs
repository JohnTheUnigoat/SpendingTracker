﻿using BL.Mappers;
using BL.Model.Transaction;
using Core.Const;
using Core.Exceptions;
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
                    toDate = currDate;
                    break;
                default: throw new ArgumentException($"reportPeriod value should be one of: {string.Join(", ", ReportPeriods.GetValues())}.");
            }

            return (fromDate, toDate);
        }

        public async Task<IEnumerable<ShortTransactionDomain>> GetTransactionsAsync(GetTransactionsDto dto)
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

            try
            {
                (fromDate, toDate) = GetReportPeriodLimits(reportPeriod);
            }
            catch (ArgumentException e)
            {
                throw new ValidationException(new() { { nameof(dto.ReportPeriod), e.Message } });
            }

            IQueryable<ShortTransactionDomain> categoryTransactions = _dbContext.Transactions
                .Where(t =>
                    t is CategoryTransaction &&
                    t.WalletId == dto.WalletId)
                .Select(t => new ShortTransactionDomain
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Target = (t as CategoryTransaction).Category.Name,
                    Timestamp = t.TimeStamp
                });

            IQueryable<ShortTransactionDomain> outgoingWalletTransaction = _dbContext.Transactions
                .Where(t =>
                    t is WalletTransaction &&
                    t.WalletId == dto.WalletId)
                .Select(t => new ShortTransactionDomain
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Target = (t as WalletTransaction).TargetWallet.Name,
                    Timestamp = t.TimeStamp
                });

            IQueryable<ShortTransactionDomain> incomingWalletTransaction = _dbContext.Transactions
                .Where(t =>
                    t is WalletTransaction &&
                    (t as WalletTransaction).TargetWalletId == dto.WalletId)
                .Select(t => new ShortTransactionDomain
                {
                    Id = t.Id,
                    Amount = t.Amount * -1,
                    Target = t.Wallet.Name,
                    Timestamp = t.TimeStamp
                });

            List<ShortTransactionDomain> res = await
                categoryTransactions
                    .Union(outgoingWalletTransaction)
                    .Union(incomingWalletTransaction)
                    .Where(t => t.Timestamp >= fromDate && t.Timestamp < toDate)
                    .OrderBy(t => t.Timestamp)
                .ToListAsync();

            return res;
        }

        public async Task<int> AddTransactionAsync(AddTransactionDtoBase dto)
        {
            if (dto.Amount == 0)
            {
                throw new ValidationException(new()
                {
                    { nameof(dto.Amount), "Transaction amount can't be 0." }
                });
            }

            if (_dbContext.Wallets.Any(w => w.Id == dto.WalletId) == false)
            {
                throw new HttpStatusException(404);
            }

            TransactionBase newTransaction;

            switch (dto)
            {
                case AddCategoryTransactionDto categoryDto:
                    newTransaction = categoryDto.ToEntity();
                    break;
                case AddWalletTransactionDto walletDto:
                    if (walletDto.Amount < 0)
                    {
                        throw new ValidationException(new()
                        {
                            { nameof(dto.Amount), "Transaction amount can't be negative when transfering to a wallet." }
                        });
                    }

                    // Make the amount negative to indicate that money was removed from this wallet and transfered to a different one
                    walletDto.Amount *= -1;

                    newTransaction = walletDto.ToEntity();
                    break;
                default: throw new ArgumentException("Unknown/unhandled addTransactionDto type.");
            }

            newTransaction.TimeStamp = DateTime.Now;

            _dbContext.Transactions.Add(newTransaction);

            await _dbContext.SaveChangesAsync();

            return newTransaction.Id;
        }
    }
}
