using System.Collections.Generic;

namespace BL.Model.Transaction
{
    public class OneWaySummaryDomain
    {
        public IEnumerable<CategorySummaryDomain> Categories { get; set; }

        public IEnumerable<WalletSummaryDomain> Wallets { get; set; }
    }
}
