using BL.Model.Transaction;
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

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IEnumerable<TransactionResponse>> GetTransactions(int walletId, [FromQuery] GetTransactionRequest request)
        {
            List<ShortTransactionDomain> domains = await _transactionService.GetTransactionsAsync(request.ToDto(walletId));

            return domains.AllToResponse();
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
