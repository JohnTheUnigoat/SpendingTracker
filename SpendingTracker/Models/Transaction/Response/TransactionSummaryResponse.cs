namespace SpendingTracker.Models.Transaction.Response
{
    public class TransactionSummaryResponse
    {
        public decimal TotalIncome { get; set; }

        public decimal TotalExpense { get; set; }

        public OneWaySummaryResponse IncomeDetails { get; set; }

        public OneWaySummaryResponse ExpenseDetails { get; set; }
    }
}
