using System.Threading.Tasks;

namespace BL.Services
{
    public interface IWalletService
    {
        Task<bool> IsUserAuthorizedForWallet(int walletId, int userId);
    }
}
