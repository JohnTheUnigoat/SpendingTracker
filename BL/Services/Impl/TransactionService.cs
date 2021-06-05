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

        private readonly IWalletService _walletService;

        public TransactionService(AppDbContext dbContext, IWalletService walletService)
        {
            _dbContext = dbContext;
            _walletService = walletService;
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

            if (dto.ReportPeriod != ReportPeriods.Custom)
            {
                await _walletService.SetWalletReportPeriodAsync(dto.WalletId, dto.ReportPeriod);
            }

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

            var nativeWalletInfos = transactions
                .Where(t => t is WalletTransaction && t.WalletId == dto.WalletId)
                .Select(t => new
                {
                    t.Id,
                    t.Amount,
                    OtherWalletId = (t as WalletTransaction).OtherWalletId,
                    TargetLabel = (t as WalletTransaction).OtherWallet.Name,
                    t.TimeStamp,
                    IsWalletShared =
                        (t as WalletTransaction).OtherWallet.UserId == dto.UserId ||
                        (t as WalletTransaction).OtherWallet.WalletAllowedUsers.Any(wu => wu.UserId == dto.UserId)
                });

            var relatedWalletInfos = transactions
                .Where(t => t is WalletTransaction && t.WalletId != dto.WalletId)
                .Select(t => new
                {
                    t.Id,
                    t.Amount,
                    OtherWalletId = t.WalletId,
                    TargetLabel = t.Wallet.Name,
                    t.TimeStamp,
                    IsWalletShared =
                        (t as WalletTransaction).Wallet.UserId == dto.UserId ||
                        (t as WalletTransaction).Wallet.WalletAllowedUsers.Any(wu => wu.UserId == dto.UserId)
                });

            IQueryable<TransactionDomain> walletTransactions = nativeWalletInfos
                .Union(relatedWalletInfos)
                .Select(x => new TransactionDomain
                {
                    Id = x.Id,
                    Amount = x.Amount,
                    CategoryId = null,
                    OtherWalletId = x.IsWalletShared ? x.OtherWalletId : null,
                    TargetLabel = x.IsWalletShared ? x.TargetLabel : "Private wallet",
                    Timestamp = x.TimeStamp
                });

            List<TransactionDomain> res = await categoryTransactions
                    .Union(walletTransactions)
                    .OrderByDescending(t => t.Timestamp)
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

                var categoryBelongsToWalletOwner = await _dbContext.Categories
                    .Where(c => c.Id == cDto.CaterodyId)
                    .Join(
                        _dbContext.Wallets.Where(w => w.Id == cDto.WalletId),
                        c => c.UserId,
                        w => w.UserId,
                        (c, w) => 1)
                    .AnyAsync();

                if (categoryBelongsToWalletOwner == false)
                {
                    throw new ValidationException(new() { { nameof(cDto.CaterodyId), "Category should belong to the wallet's owner." } });
                }

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

                bool userAuthorizedForOtherWallet = await _dbContext.Wallets
                    .Where(w =>
                        w.Id == wDto.OtherWalletId &&
                        (w.UserId == dto.UserId || w.WalletAllowedUsers.Any(wu => wu.UserId == dto.UserId)))
                    .AnyAsync();

                if (userAuthorizedForOtherWallet == false)
                {
                    throw new HttpStatusException(404, "other wallet not found.");
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

            if (dto.ReportPeriod != ReportPeriods.Custom)
            {
                await _walletService.SetWalletReportPeriodAsync(dto.WalletId, dto.ReportPeriod);
            }

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

            if (dto.ReportPeriod != ReportPeriods.Custom)
            {
                await _walletService.SetWalletReportPeriodAsync(dto.WalletId, dto.ReportPeriod);
            }

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
                    WalletId = (t as WalletTransaction).OtherWalletId,
                    Name = (t as WalletTransaction).OtherWallet.Name,
                    Amount = t.Amount,
                    IsWalletShared = 
                        (t as WalletTransaction).OtherWallet.UserId == dto.UserId ||
                        (t as WalletTransaction).OtherWallet.WalletAllowedUsers.Any(wu => wu.UserId == dto.UserId)
                });

            var relatedWalletInfos = transactions
                .Where(t => t.WalletId != dto.WalletId)
                .Select(t => new
                {
                    WalletId = t.WalletId,
                    Name = t.Wallet.Name,
                    Amount = t.Amount * -1,
                    IsWalletShared =
                        t.Wallet.UserId == dto.UserId ||
                        t.Wallet.WalletAllowedUsers.Any(wu => wu.UserId == dto.UserId)
                });

            var walletInfosWithUnshared = nativeWalletInfos
                .Union(relatedWalletInfos)
                .GroupBy(x => new { x.WalletId, x.Name, x.IsWalletShared })
                .Select(g => new
                {
                    g.Key.WalletId,
                    g.Key.Name,
                    Amount = g.Sum(x => x.Amount),
                    g.Key.IsWalletShared
                })
                .Where(x => x.Amount != 0)
                .Select(x => new
                {
                    x.WalletId,
                    x.Name,
                    x.Amount,
                    IsIncome = x.Amount > 0,
                    x.IsWalletShared
                });

            var walletInfosOnlyShared = walletInfosWithUnshared
                .Where(x => x.IsWalletShared)
                .Select(x => new
                {
                    WalletId = x.WalletId as int?,
                    CategoryId = null as int?,
                    Name = x.Name,
                    Amount = x.Amount
                });

            var walletInfosOnlyUnshared = walletInfosWithUnshared
                .Where(x => x.IsWalletShared == false)
                .GroupBy(x => new { x.IsWalletShared, x.IsIncome})
                .Select(g => new
                {
                    WalletId = null as int?,
                    CategoryId = null as int?,
                    Name = "Private wallet(s)",
                    Amount = g.Sum(x => x.Amount)
                });

            var walletInfos = walletInfosOnlyShared.Union(walletInfosOnlyUnshared);

            var allInfos = await categoryInfos
                .Union(walletInfos)
                .ToListAsync();

            var categoryIncomes = allInfos
                .Where(i => i.CategoryId != null && i.Amount > 0)
                .Select(i => new CategorySummaryDomain
                {
                    Id = i.CategoryId.Value,
                    Name = i.Name,
                    Amount = i.Amount
                });

            var categoryExpenses = allInfos
                .Where(i => i.CategoryId != null && i.Amount < 0)
                .Select(i => new CategorySummaryDomain
                {
                    Id = i.CategoryId.Value,
                    Name = i.Name,
                    Amount = i.Amount
                });

            var walletIncomes = allInfos
                .Where(i => i.CategoryId == null && i.Amount > 0)
                .Select(i => new WalletSummaryDomain
                {
                    Id = i.WalletId,
                    Name = i.Name,
                    Amount = i.Amount
                });

            var walletExpenses = allInfos
                .Where(i => i.CategoryId == null && i.Amount < 0)
                .Select(i => new WalletSummaryDomain
                {
                    Id = i.WalletId,
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
                IncomeDetails = new OneWaySummaryDomain
                {
                    Categories = categoryIncomes,
                    Wallets = walletIncomes
                },
                ExpenseDetails = new OneWaySummaryDomain
                {
                    Categories = categoryExpenses,
                    Wallets = walletExpenses
                }
            };
        }
    }
}
