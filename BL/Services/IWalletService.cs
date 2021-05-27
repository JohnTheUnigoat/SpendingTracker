using BL.Model.Wallet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IWalletService
    {
        Task<bool> IsUserAuthorizedForWalletAsync(int walletId, int userId);

        Task<bool> IsTransactionInWalletAsync(int walletId, int transactionId);

        Task<IEnumerable<WalletDomain>> GetWalletsAsync(int userId);

        Task<int> AddWalletAsync(AddWalletDto dto);

        Task RenameWalletAsync(int walletId, string name);

        Task DeleteWalletAsync(int walletId);
    }
}
