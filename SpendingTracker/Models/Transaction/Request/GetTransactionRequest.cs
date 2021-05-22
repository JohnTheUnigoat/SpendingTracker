using System;

namespace SpendingTracker.Models.Transaction.Request
{
    public class GetTransactionRequest
    {
        public string ReportPeriod { get; set; }

        public DateTime? From { get; set; }

        public DateTime? To { get; set; }
    }
}
