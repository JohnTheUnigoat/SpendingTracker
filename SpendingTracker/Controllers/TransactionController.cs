using BL.Model.Transaction;
using BL.Services;
using Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Mappers;
using SpendingTracker.Models.Transaction.Request;
using SpendingTracker.Models.Transaction.Response;
using System;
using System.Collections.Generic;
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

        [HttpGet]
        public async Task<IEnumerable<TransactionResponse>> GetTransactions(int walletId, [FromQuery] GetTransactionRequest request)
        {
            List<TransactionDomain> domains = await _transactionService.GetTransactionsAsync(request.ToDto(walletId));

            return domains.AllToResponse();
        }

        [HttpGet("summary")]
        public async Task<ShortTransactionSummaryDomain> GetTransactionSummary(int walletId, [FromQuery] GetTransactionRequest request)
        {
            return await _transactionService.GetShortSummaryAsync(request.ToDto(walletId));
        }

        [HttpPost]
        public async Task<int> AddTransaction(int walletId, [FromBody] AddUpdateTransactionRequest request)
        {
            if (request.OtherWalletId.HasValue && request.CategoryId.HasValue)
            {
                throw new ValidationException(new()
                {
                    {
                        nameof(request.OtherWalletId),
                        $"Only one of the fields {nameof(request.OtherWalletId)}, {nameof(request.CategoryId)} should have a value."
                    },
                    {
                        nameof(request.CategoryId),
                        $"Only one of the fields {nameof(request.OtherWalletId)}, {nameof(request.CategoryId)} should have a value."
                    },
                });
            }

            AddUpdateTransactionDtoBase dto;

            if (request.CategoryId.HasValue)
                dto = request.ToCategoryDto(walletId);
            else
                dto = request.ToWalletDto(walletId);

            return await _transactionService.AddTransactionAsync(dto);
        }

        [HttpPut("{transactionId:int}")]
        public async Task<TransactionResponse> UpdateTransaction(
            int walletId, int transactionId, [FromBody] AddUpdateTransactionRequest request)
        {
            if (await _walletService.IsTransactionInWalletAsync(walletId, transactionId) == false)
            {
                throw new HttpStatusException(404);
            }

            if (request.OtherWalletId.HasValue && request.CategoryId.HasValue)
            {
                throw new ValidationException(new()
                {
                    {
                        nameof(request.OtherWalletId),
                        $"Only one of the fields {nameof(request.OtherWalletId)}, {nameof(request.CategoryId)} should have a value."
                    },
                    {
                        nameof(request.CategoryId),
                        $"Only one of the fields {nameof(request.OtherWalletId)}, {nameof(request.CategoryId)} should have a value."
                    },
                });
            }

            AddUpdateTransactionDtoBase dto;

            if (request.CategoryId.HasValue)
                dto = request.ToCategoryDto(walletId);
            else
                dto = request.ToWalletDto(walletId);

            return (await _transactionService.UpdateTransactionAsync(transactionId, dto)).ToResponse();
        }

        [HttpDelete("{transactionId:int}")]
        public async Task DeleteTransaction(int walletId, int transactionId)
        {
            if (await _walletService.IsTransactionInWalletAsync(walletId, transactionId) == false)
            {
                throw new HttpStatusException(404);
            }

            await _transactionService.DeleteTransactionAsync(transactionId);
        }
    }
}
