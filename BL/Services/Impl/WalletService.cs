using Core.Exceptions;
using DAL_EF;
using DAL_EF.Entity;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> IsUserAuthorizedForWallet(int walletId, int userId)
        {
            Wallet wallet = await _dbContext.Wallets.FindAsync(walletId);

            if (wallet == null)
            {
                throw new HttpStatusException(404);
            }

            return wallet.UserId == userId;
        }
    }
}
