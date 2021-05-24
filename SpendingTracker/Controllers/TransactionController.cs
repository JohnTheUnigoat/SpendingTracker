﻿using BL.Model.Transaction;
using BL.Services;
using Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Mappers;
using SpendingTracker.Models.Transaction.Request;
using SpendingTracker.Models.Transaction.Response;
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

        [HttpPost]
        public async Task<int> AddTransaction(int walletId, [FromBody] AddUpdateTransactionRequest request)
        {
            if (request.SourceWalletId.HasValue && request.CategoryId.HasValue)
            {
                throw new ValidationException(new()
                {
                    {
                        nameof(request.SourceWalletId),
                        $"Only one of the fields {nameof(request.SourceWalletId)}, {nameof(request.CategoryId)} should have a value."
                    },
                    {
                        nameof(request.CategoryId),
                        $"Only one of the fields {nameof(request.SourceWalletId)}, {nameof(request.CategoryId)} should have a value."
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

        [HttpPut]
        public async Task<TransactionResponse> UpdateTransaction(
            int walletId, int transactionId, [FromBody] AddUpdateTransactionRequest request)
        {
            if (await _walletService.IsTransactionInWalletAsync(walletId, transactionId) == false)
            {
                throw new HttpStatusException(404);
            }

            if (request.SourceWalletId.HasValue && request.CategoryId.HasValue)
            {
                throw new ValidationException(new()
                {
                    {
                        nameof(request.SourceWalletId),
                        $"Only one of the fields {nameof(request.SourceWalletId)}, {nameof(request.CategoryId)} should have a value."
                    },
                    {
                        nameof(request.CategoryId),
                        $"Only one of the fields {nameof(request.SourceWalletId)}, {nameof(request.CategoryId)} should have a value."
                    },
                });
            }

            AddUpdateTransactionDtoBase dto;

            if (request.CategoryId.HasValue)
                dto = request.ToCategoryDto(walletId);
            else
                dto = request.ToWalletDto(walletId);

            return (await _transactionService.UpdateTransaction(transactionId, dto)).ToResponse();
        }

        [HttpDelete]
        public async Task DeleteTransaction(int walletId, int transactionId)
        {
            if (await _walletService.IsTransactionInWalletAsync(walletId, transactionId) == false)
            {
                throw new HttpStatusException(404);
            }

            await _transactionService.DeleteTransaction(transactionId);
        }
    }
}
