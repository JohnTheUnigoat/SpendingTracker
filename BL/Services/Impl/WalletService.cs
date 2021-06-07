using BL.Mappers;
using BL.Model.Wallet;
using Core.Exceptions;
using DAL_EF;
using DAL_EF.Entity;
using DAL_EF.Entity.Transaction;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Services.Impl
{
    public class WalletService : IWalletService
    {
        private readonly AppDbContext _dbContext;

        public WalletService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> IsUserAuthorizedForWalletAsync(int walletId, int userId)
        {
            Wallet wallet = await _dbContext.Wallets
                .AsNoTracking()
                .Include(w => w.WalletAllowedUsers)
                .SingleOrDefaultAsync(w => w.Id == walletId);

            if (wallet == null)
            {
                throw new HttpStatusException(404);
            }

            return wallet.UserId == userId || wallet.WalletAllowedUsers.Any(wu => wu.UserId == userId);
        }

        public async Task<IEnumerable<WalletDomain>> GetWalletsAsync(int userId)
        {
            return await _dbContext.Wallets
                .Where(w => w.UserId == userId || w.WalletAllowedUsers.Any(wu => wu.UserId == userId))
                .Select(w => new WalletDomain
                {
                    Id = w.Id,
                    OwnerEmail = w.User.Email,
                    SharedEmails = w.UserId == userId ? w.WalletAllowedUsers.Select(wu => wu.User.Email).ToList() : null,
                    Name = w.Name,
                    DefaultReportPeriod = w.DefaultReportPeriod
                })
                .ToListAsync();
        }

        public async Task<int> AddWalletAsync(AddWalletDto dto)
        {
            Wallet newWallet = dto.ToEntity();

            _dbContext.Wallets.Add(newWallet);

            await _dbContext.SaveChangesAsync();

            return newWallet.Id;
        }

        public async Task UpdateWalletAsync(UpdateWalletDto dto)
        {
            if (_dbContext.Wallets.Any(w => w.Id == dto.WalletId) == false)
            {
                throw new HttpStatusException(404);
            }

            if (dto.Name == "")
            {
                throw new ValidationException(new() { { nameof(dto.Name), "Name can't be empty." } });
            }

            List<int> userIds = await _dbContext.Users
                .Where(u => dto.SharedEmails.Contains(u.Email))
                .Select(u => u.Id)
                .ToListAsync();

            if (userIds.Count() < dto.SharedEmails.Count())
            {
                throw new ValidationException(new() { { nameof(dto.SharedEmails), "Some/all emails are not associated with a user." } });
            }

            List<int> currentUserIds = await _dbContext.Set<WalletAllowedUser>()
                .Where(wu => wu.WalletId == dto.WalletId)
                .Select(wu => wu.UserId)
                .ToListAsync();

            var newUserAssociations = userIds
                .Where(uid => currentUserIds.Contains(uid) == false)
                .Select(uid => new WalletAllowedUser
                {
                    WalletId = dto.WalletId,
                    UserId = uid
                });

            var deletedUserAssociations = currentUserIds
                .Where(uid => userIds.Contains(uid) == false)
                .Select(uid => new WalletAllowedUser
                {
                    WalletId = dto.WalletId,
                    UserId = uid
                });

            _dbContext.Set<WalletAllowedUser>().RemoveRange(deletedUserAssociations);
            _dbContext.Set<WalletAllowedUser>().AddRange(newUserAssociations);

            var wallet = new Wallet
            {
                Id = dto.WalletId
            };

            _dbContext.Attach(wallet);

            wallet.Name = dto.Name;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsTransactionInWalletAsync(int walletId, int transactionId)
        {
            TransactionBase transaction = await _dbContext.Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == transactionId);

            if (transaction == null) return false;

            if (transaction is WalletTransaction)
            {
                WalletTransaction wt = transaction as WalletTransaction;

                return wt.WalletId == walletId || wt.OtherWalletId == walletId;
            }

            return transaction.WalletId == walletId;
        }

        public async Task DeleteWalletAsync(int walletId)
        {
            var wallet = new Wallet
            {
                Id = walletId
            };

            Task<int[]> transactionIdsToRemoveTask = _dbContext.Transactions
                .Where(t =>
                    t.WalletId == walletId ||
                    t is WalletTransaction && (t as WalletTransaction).OtherWalletId == walletId)
                .Select(t => t.Id ).ToArrayAsync();

            _dbContext.Transactions.RemoveRange((await transactionIdsToRemoveTask).Select(id => new CategoryTransaction { Id = id }));

            _dbContext.Remove(wallet);

            await _dbContext.SaveChangesAsync();
        }

        public async Task SetWalletReportPeriodAsync(int walletId, string reportPeriod)
        {
            var wallet = new Wallet
            {
                Id = walletId
            };

            _dbContext.Attach(wallet);

            wallet.DefaultReportPeriod = reportPeriod;

            await _dbContext.SaveChangesAsync();

            _dbContext.Entry(wallet).State = EntityState.Detached;
        }
    }
}
