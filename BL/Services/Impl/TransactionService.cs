using BL.Mappers;
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

        public async Task<IEnumerable<ShortTransactionDomain>> GetTransactionsAsync(GetTransactionsDto dto)
        {
            Wallet wallet = _dbContext.Wallets.Find(dto.WalletId);

            if (wallet == null)
            {
                throw new HttpStatusException(404);
            }

            DateTime currDate = DateTime.Now;

            DateTime fromDate;
            DateTime toDate;

            string reportPeriod = string.IsNullOrEmpty(dto.ReportPeriod) ?
                wallet.DefaultReportPeriod :
                dto.ReportPeriod;

            switch (reportPeriod)
            {
                case ReportPeriods.CurrentDay:
                    toDate = new DateTime(currDate.Year, currDate.Month, currDate.Day);
                    fromDate = toDate.AddHours(24);
                    break;
                case ReportPeriods.CurrentWeek:
                    toDate = new DateTime(
                        currDate.Year,
                        currDate.Month,
                        currDate.Day + (7 - (int)currDate.DayOfWeek));

                    fromDate = toDate.AddDays(-7);
                    break;
                case ReportPeriods.CurrentMonth:
                    toDate = new DateTime(currDate.Year, currDate.Month + 1, 1);
                    fromDate = toDate.AddMonths(-1);
                    break;
                case ReportPeriods.CurrentYear:
                    toDate = new DateTime(currDate.Year + 1, 1, 1);
                    fromDate = toDate.AddYears(-1);
                    break;
                case ReportPeriods.AllTime:
                    toDate = currDate;
                    fromDate = new DateTime(0);
                    break;
                default: throw new ValidationException(new()
                {
                    { nameof(dto.ReportPeriod), $"reportPeriod value should be one of: {string.Join(", ", ReportPeriods.GetValues())}." }
                });
            }

            List<ShortTransactionDomain> res = await _dbContext.Transactions
                .Where(t =>
                    t.SourceWalletId == dto.WalletId &&
                    t.TimeStamp >= fromDate &&
                    t.TimeStamp < toDate)
                .Select(t => new ShortTransactionDomain
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    Target = (t is CategoryTransaction) ?
                        (t as CategoryTransaction).Category.Name :
                        (t as WalletTransaction).TargetWallet.Name,
                    Timestamp = t.TimeStamp
                })
                .ToListAsync();

            return res;
        }

        public async Task<int> AddTransactionAsync(AddTransactionDtoBase dto)
        {
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
