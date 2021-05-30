using System.Collections.Generic;

namespace SpendingTracker.Models.Transaction.Response
{
    public class OneWaySummaryResponse
    {
        public IEnumerable<CategorySummaryResponse> Categories { get; set; }

        public IEnumerable<WalletSummaryResponse> Wallets { get; set; }
    }
}
