using BL.Model.Wallet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface IWalletService
    {
        Task<bool> IsUserAuthorizedForWalletAsync(int walletId, int userId);

        Task<IEnumerable<WalletDomain>> GetWalletsAsync(int userId);

        Task<int> AddWalletAsync(AddWalletDto dto);

        Task RenameWalletAsync(int walletId, string name);
    }
}
