using System;

namespace SpendingTracker.Models.Transaction.Request
{
    public class AddUpdateTransactionRequest
    {
        public decimal Amount { get; set; }

        public int? OtherWalletId { get; set; }

        public int? CategoryId { get; set; }

        public DateTime? ManualTimestamp { get; set; }
    }
}
