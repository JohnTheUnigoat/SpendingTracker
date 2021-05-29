namespace SpendingTracker.Models.Transaction.Response
{
    public class CategoryOrWalletSummaryResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }
    }
}
