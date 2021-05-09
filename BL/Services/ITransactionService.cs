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
        Task<IEnumerable<ShortTransactionDomain>> GetTransactionsAsync(GetTransactionsDto dto);
    }
}
