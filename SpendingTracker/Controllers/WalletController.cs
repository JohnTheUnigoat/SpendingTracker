using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Mappers;
using SpendingTracker.Models.Wallet.Request;
using SpendingTracker.Models.Wallet.Response;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task RenameWallet(int walletId, [FromBody] string name)
        {
            await _walletService.RenameWalletAsync(walletId, name);
        }
    }
}
