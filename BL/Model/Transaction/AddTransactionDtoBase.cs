using System;

namespace BL.Model.Transaction
{
    public abstract class AddTransactionDtoBase
    {
        public int WalletId { get; set; }

        public decimal Amount { get; set; }

        public DateTime? ManualTimestamp { get; set; }
    }
}
