namespace BL.Model.Transaction
{
    public class TransactionSummaryDomain
    {
        public decimal TotalIncome { get; set; }

        public decimal TotalExpense { get; set; }

        public OneWayTransactionSummaryDomain IncomeDetails { get; set; }

        public OneWayTransactionSummaryDomain ExpenseDetails { get; set; }
    }
}
