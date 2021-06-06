using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Mappers;
using SpendingTracker.Models.Wallet.Request;
using SpendingTracker.Models.Wallet.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendingTracker.Controllers
{
    [Route("api/wallets")]
    [ApiController]
    [Authorize]
    public class WalletController : BaseController
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        [HttpGet]
        public async Task<IEnumerable<WalletResponse>> GetWallets()
        {
            return (await _walletService.GetWalletsAsync(UserId)).AllToResponse();
        }

        [HttpPost]
        public async Task<int> AddWallet(AddWalletRequest request)
        {
            return await _walletService.AddWalletAsync(request.ToDto(UserId));
        }

        [HttpPut("{walletId:int}")]
        public async Task RenameWallet(int walletId, [FromBody] RenameWalletRequest request)
        {
            await _walletService.RenameWalletAsync(walletId, request.Name);
        }

        [HttpDelete("{walletId:int}")]
        public async Task DeleteWallet(int walletId)
        {
            await _walletService.DeleteWalletAsync(walletId);
        }
    }
}
