using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Models.Transaction.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpendingTracker.Controllers
{
    [Route("api/wallets/{walletId:int}/transactions")]
    [ApiController]
    [Authorize]
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _transactionService;
        private readonly IWalletService _walletService;

        public TransactionController(
            ITransactionService transactionService,
            IWalletService walletService)
        {
            _transactionService = transactionService;
            _walletService = walletService;
        }

        //[HttpPost]
        //public async Task<int> AddTransaction(int walletId, [FromBody] AddTransactionRequest request)
        //{


        //    return await _transactionService.AddTransactionAsync()
        //}
    }
}
