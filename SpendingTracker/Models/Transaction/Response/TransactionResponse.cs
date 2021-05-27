using System;

namespace SpendingTracker.Models.Transaction.Response
{
    public class TransactionResponse
    {
        public int Id { get; set; }

        public int? CategoryId { get; set; }

        public int? OtherWalletId { get; set; }

        public string TargetLabel { get; set; }

        public decimal Amount { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
