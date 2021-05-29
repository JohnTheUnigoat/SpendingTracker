using System.Collections.Generic;

namespace BL.Model.Transaction
{
    public class OneWayTransactionSummaryDomain
    {
        public IEnumerable<CategoryOrWalletSummaryDomain> Categories { get; set; }

        public IEnumerable<CategoryOrWalletSummaryDomain> Wallets { get; set; }
    }
}
