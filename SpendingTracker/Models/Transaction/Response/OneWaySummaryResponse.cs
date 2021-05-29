using System.Collections.Generic;

namespace SpendingTracker.Models.Transaction.Response
{
    public class OneWaySummaryResponse
    {
        public IEnumerable<CategoryOrWalletSummaryResponse> Categories { get; set; }

        public IEnumerable<CategoryOrWalletSummaryResponse> Wallets { get; set; }
    }
}
