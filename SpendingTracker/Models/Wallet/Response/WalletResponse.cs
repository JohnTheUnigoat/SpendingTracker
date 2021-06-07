using System.Collections.Generic;

namespace SpendingTracker.Models.Wallet.Response
{
    public class WalletResponse
    {
        public int Id { get; set; }

        public string OwnerEmail { get; set; }

        public IEnumerable<string> SharedEmails { get; set; }

        public string Name { get; set; }

        public string DefaultReportPeriod { get; set; }
    }
}
