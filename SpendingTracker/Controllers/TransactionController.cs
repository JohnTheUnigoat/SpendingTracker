using BL.Model.Transaction;
using BL.Services;
using Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Mappers;
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

        [HttpPost]
        public async Task<int> AddTransaction(int walletId, [FromBody] AddTransactionRequest request)
        {
            if (request.TargetWalletId.HasValue && request.CategoryId.HasValue)
            {
                throw new ValidationException(new()
                {
                    {
                        nameof(request.TargetWalletId),
                        $"Only one of the fields {nameof(request.TargetWalletId)}, {nameof(request.CategoryId)} should have a value."
                    },
                    {
                        nameof(request.CategoryId),
                        $"Only one of the fields {nameof(request.TargetWalletId)}, {nameof(request.CategoryId)} should have a value."
                    },
                });
            }

            AddTransactionDtoBase dto;

            if (request.CategoryId.HasValue)
                dto = request.ToCategoryDto(walletId);
            else
                dto = request.ToWalletDto(walletId);

            return await _transactionService.AddTransactionAsync(dto);
        }
    }
}
