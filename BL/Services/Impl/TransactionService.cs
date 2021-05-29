using BL.Mappers;
using BL.Model.Transaction;
using Core.Const;
using Core.Exceptions;
using Core.Exceptions.CustomExceptions;
using DAL_EF;
using DAL_EF.Entity.Transaction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private async Task<string> GetDefaultReportPriod(int walletId)
        {
            return await _dbContext.Wallets
                .Where(w => w.Id == walletId)
                .Select(w => w.DefaultReportPeriod)
                .FirstAsync();
        }

        private (DateTime from, DateTime to) GetReportPeriodLimits(GetTransactionsDto dto)
        {
            if (dto.ReportPeriod == ReportPeriods.Custom)
            {
                return (dto.CustomFromDate.Value, dto.CustomToDate.Value);
            }

            DateTime currDate = DateTime.Today;

            DateTime fromDate, toDate;

            switch (dto.ReportPeriod)
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

        /// <summary>
        /// Gets an IQueryable with all transactions, filtered by report period and being related to walletId specified in GetTransactionsDto
        /// </summary>
        private IQueryable<TransactionBase> GetFilteredTransactions(GetTransactionsDto dto)
        {
            DateTime fromDate, toDate;

            try
            {
                (fromDate, toDate) = GetReportPeriodLimits(dto);
            }
            catch (ArgumentException e)
            {
                throw new ValidationException(new() { { nameof(dto.ReportPeriod), e.Message } });
            }

            var transactionsInPeriod = _dbContext.Transactions
                .Where(t => t.TimeStamp >= fromDate && t.TimeStamp < toDate);

            var inWalletTransactions = transactionsInPeriod
                .Where(t => t.WalletId == dto.WalletId);

            var otherWalletTransactions = transactionsInPeriod
                .Where(t =>
                    t is WalletTransaction &&
                    (t as WalletTransaction).OtherWalletId == dto.WalletId);

            return inWalletTransactions.Union(otherWalletTransactions);
        }

        public async Task<List<TransactionDomain>> GetTransactionsAsync(GetTransactionsDto dto)
        {
            var transactions = GetFilteredTransactions(dto);

            IQueryable<TransactionDomain> categoryTransactions = transactions
                .Where(t => t is CategoryTransaction)
                .Select(t => new TransactionDomain
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    CategoryId = (t as CategoryTransaction).CategoryId,
                    OtherWalletId = null,
                    TargetLabel = (t as CategoryTransaction).Category.Name,
                    Timestamp = t.TimeStamp
                });

            IQueryable<TransactionDomain> outgoingWalletTransaction = transactions
                .Where(t => t is WalletTransaction)
                .Select(t => new TransactionDomain
                {
                    Id = t.Id,
                    Amount = t.Amount,
                    CategoryId = null,
                    OtherWalletId = (t as WalletTransaction).OtherWalletId,
                    TargetLabel = (t as WalletTransaction).OtherWallet.Name,
                    Timestamp = t.TimeStamp
                });

            IQueryable<TransactionDomain> incomingWalletTransaction = transactions
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
                    .OrderBy(t => t.Timestamp)
                .ToListAsync();

            return res;
        }

        private async Task ValidateTransactionDtoAsync(AddUpdateTransactionDtoBase dto)
        {
            if (dto.Amount == 0)
            {
                throw new ValidationException(new()
                {
                    { nameof(dto.Amount), "Transaction amount can't be 0." }
                });
            }

            if (dto is AddUpdateCategoryTransactionDto)
            {
                var cDto = dto as AddUpdateCategoryTransactionDto;

                var targetCategory = await _dbContext.Categories
                    .AsNoTracking()
                    .FirstAsync(c => c.Id == cDto.CaterodyId);

                if (targetCategory.IsIncome && cDto.Amount < 0)
                {
                    throw new ValidationException(new()
                    {
                        { nameof(cDto.Amount), "The transaction value should be positive for an income category." }
                    });
                }

                if (targetCategory.IsIncome == false && cDto.Amount > 0)
                {
                    throw new ValidationException(new()
                    {
                        { nameof(cDto.Amount), "The transaction value should be negative for an expense category." }
                    });
                }
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
            await ValidateTransactionDtoAsync(dto);

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

        public async Task<TransactionDomain> UpdateTransactionAsync(int transactionId, AddUpdateTransactionDtoBase dto)
        {
            await ValidateTransactionDtoAsync(dto);

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

        public async Task DeleteTransactionAsync(int transactionId)
        {
            var thing = _dbContext.ChangeTracker.Entries();

            TransactionBase transaction = new CategoryTransaction
            {
                Id = transactionId
            };

            _dbContext.Remove(transaction);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<ShortTransactionSummaryDomain> GetShortSummaryAsync(GetTransactionsDto dto)
        {
            var transactions = GetFilteredTransactions(dto);

            var categoryAmounts = transactions
                .Where(t => t is CategoryTransaction)
                .Select(t => t.Amount);

            var walletAmounts = transactions
                .Where(t => t is WalletTransaction)
                .Select(t => new
                {
                    walletId = t.WalletId == dto.WalletId ?
                        (t as WalletTransaction).OtherWalletId :
                        t.WalletId,
                    amount = t.WalletId == dto.WalletId ?
                        t.Amount :
                        t.Amount * -1
                })
                .GroupBy(x => x.walletId)
                .Select(g => g.Sum(x => x.amount));

            var allAmounts = categoryAmounts.Union(walletAmounts);

            return new ShortTransactionSummaryDomain
            {
                Income = await allAmounts.Where(a => a > 0).SumAsync(),
                Expense = await allAmounts.Where(a => a < 0).SumAsync()
            };
        }

        public async Task<TransactionSummaryDomain> GetSummaryAsync(GetTransactionsDto dto)
        {
            var transactions = GetFilteredTransactions(dto);

            var categoryInfos = transactions
                .Where(t => t is CategoryTransaction)
                .Select(t => t as CategoryTransaction)
                .Select(t => new
                {
                    t.CategoryId,
                    t.Category.Name,
                    t.Amount
                })
                .GroupBy(x => new { x.CategoryId, x.Name })
                .Select(g => new
                {
                    WalletId = null as int?,
                    CategoryId = g.Key.CategoryId as int?,
                    g.Key.Name,
                    Amount = g.Sum(x => x.Amount)
                });

            var nativeWalletInfos = transactions
                .Where(t => t is WalletTransaction && t.WalletId == dto.WalletId)
                .Select(t => new
                {
                    (t as WalletTransaction).OtherWalletId,
                    (t as WalletTransaction).OtherWallet.Name,
                    t.Amount
                })
                .GroupBy(x => new { x.OtherWalletId, x.Name })
                .Select(g => new
                {
                    WalletId = g.Key.OtherWalletId as int?,
                    CategoryId = null as int?,
                    g.Key.Name,
                    Amount = g.Sum(x => x.Amount)
                });

            var relatedWalletInfos = transactions
                .Where(t => t.WalletId != dto.WalletId)
                .Select(t => new
                {
                    t.WalletId,
                    t.Wallet.Name,
                    t.Amount
                })
                .GroupBy(x => new { x.WalletId, x.Name })
                .Select(g => new
                {
                    WalletId = g.Key.WalletId as int?,
                    CategoryId = null as int?,
                    g.Key.Name,
                    Amount = g.Sum(x => x.Amount) * -1
                })
                .Where(i => i.Amount != 0);

            var allInfos = await categoryInfos
                .Union(nativeWalletInfos)
                .Union(relatedWalletInfos)
                .ToListAsync();

            var categoryIncomes = allInfos
                .Where(i => i.CategoryId != null && i.Amount > 0)
                .Select(i => new CategoryOrWalletSummaryDomain
                {
                    Id = i.CategoryId.Value,
                    Name = i.Name,
                    Amount = i.Amount
                });

            var categoryExpenses = allInfos
                .Where(i => i.CategoryId != null && i.Amount < 0)
                .Select(i => new CategoryOrWalletSummaryDomain
                {
                    Id = i.CategoryId.Value,
                    Name = i.Name,
                    Amount = i.Amount
                });

            var walletIncomes = allInfos
                .Where(i => i.WalletId != null && i.Amount > 0)
                .Select(i => new CategoryOrWalletSummaryDomain
                {
                    Id = i.WalletId.Value,
                    Name = i.Name,
                    Amount = i.Amount
                });

            var walletExpenses = allInfos
                .Where(i => i.WalletId != null && i.Amount < 0)
                .Select(i => new CategoryOrWalletSummaryDomain
                {
                    Id = i.WalletId.Value,
                    Name = i.Name,
                    Amount = i.Amount
                });

            return new TransactionSummaryDomain
            {
                TotalIncome = allInfos
                    .Where(i => i.Amount > 0)
                    .Sum(i => i.Amount),
                TotalExpense = allInfos
                    .Where(i => i.Amount < 0)
                    .Sum(i => i.Amount),
                IncomeDetails = new OneWayTransactionSummaryDomain
                {
                    Categories = categoryIncomes,
                    Wallets = walletIncomes
                },
                ExpenseDetails = new OneWayTransactionSummaryDomain
                {
                    Categories = categoryExpenses,
                    Wallets = walletExpenses
                }
            };
        }
    }
}
