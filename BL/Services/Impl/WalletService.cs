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
            Wallet wallet = await _dbContext.Wallets.AsNoTracking().SingleOrDefaultAsync(w => w.Id == walletId);

            if (wallet == null)
            {
                throw new HttpStatusException(404);
            }

            return wallet.UserId == userId;
        }

        public async Task<IEnumerable<WalletDomain>> GetWalletsAsync(int userId)
        {
            return await _dbContext.Wallets
                .Where(w => w.UserId == userId)
                .Select(w => new WalletDomain
                {
                    Id = w.Id,
                    Name = w.Name
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

        public async Task RenameWalletAsync(int walletId, string name)
        {
            var wallet = _dbContext.Wallets.Find(walletId);

            if (wallet == null)
            {
                throw new HttpStatusException(404);
            }

            wallet.Name = name;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsTransactionInWalletAsync(int walletId, int transactionId)
        {
            TransactionBase transaction = await _dbContext.Transactions.FindAsync(transactionId);

            if (transaction == null) return false;

            if (transaction is WalletTransaction)
            {
                WalletTransaction wt = transaction as WalletTransaction;

                return wt.WalletId == walletId || wt.SourceWalletId == walletId;
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
                    t is WalletTransaction && (t as WalletTransaction).SourceWalletId == walletId)
                .Select(t => t.Id ).ToArrayAsync();

            _dbContext.Transactions.RemoveRange((await transactionIdsToRemoveTask).Select(id => new CategoryTransaction { Id = id }));

            Task<int[]> categoriesToRemoveTask = _dbContext.Categories
                .Where(c => c.WalletId == walletId)
                .Select(c => c.Id).ToArrayAsync();

            _dbContext.Categories.RemoveRange((await categoriesToRemoveTask).Select(id => new Category { Id = id }));

            _dbContext.Remove(wallet);

            await _dbContext.SaveChangesAsync();
        }
    }
}
