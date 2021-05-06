using DAL_EF;
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
            return await _dbContext.Wallets.Where(w => w.Id == walletId && w.UserId == userId).AnyAsync();
        }
    }
}
