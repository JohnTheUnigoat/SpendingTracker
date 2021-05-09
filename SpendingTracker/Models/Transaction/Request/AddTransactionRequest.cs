namespace SpendingTracker.Models.Transaction.Request
{
    public class AddTransactionRequest
    {
        public decimal Amount { get; set; }

        public int? TargetWalletId { get; set; }

        public int? CategoryId { get; set; }
    }
}
