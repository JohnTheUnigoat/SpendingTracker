using System;

namespace BL.Model.Transaction
{
    public class GetTransactionsDto
    {
        public int WalletId { get; set; }

        public string ReportPeriod { get; set; }

        public DateTime? CustomFromDate { get; set; }

        public DateTime? CustomToDate { get; set; }
    }
}
