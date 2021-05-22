using BL.Model.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface ITransactionService
    {
        Task<List<TransactionDomain>> GetTransactionsAsync(GetTransactionsDto dto);

        Task<int> AddTransactionAsync(AddUpdateTransactionDtoBase dto);

        Task<TransactionDomain> UpdateTransaction(int transactionId, AddUpdateTransactionDtoBase dto);

        Task DeleteTransaction(int transactionId);
    }
}
