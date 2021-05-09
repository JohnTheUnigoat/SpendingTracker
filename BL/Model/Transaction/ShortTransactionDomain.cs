using System;

namespace BL.Model.Transaction
{
    public class ShortTransactionDomain
    {
        public int Id { get; set; }

        public string Target { get; set; }

        public decimal Amount { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
