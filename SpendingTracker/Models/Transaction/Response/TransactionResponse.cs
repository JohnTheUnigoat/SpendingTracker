using System;

namespace SpendingTracker.Models.Transaction.Response
{
    public class TransactionResponse
    {
        public int Id { get; set; }

        public string Target { get; set; }

        public decimal Amount { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
