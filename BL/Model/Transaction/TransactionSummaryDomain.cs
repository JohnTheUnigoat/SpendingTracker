namespace BL.Model.Transaction
{
    public class TransactionSummaryDomain
    {
        public decimal TotalIncome { get; set; }

        public decimal TotalExpense { get; set; }

        public OneWaySummaryDomain IncomeDetails { get; set; }

        public OneWaySummaryDomain ExpenseDetails { get; set; }
    }
}
