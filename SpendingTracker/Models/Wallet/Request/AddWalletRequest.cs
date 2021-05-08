using System.Collections.Generic;

namespace SpendingTracker.Models.Wallet.Request
{
    public class AddWalletRequest
    {
        public string Name { get; set; }

        public IEnumerable<string> Categories { get; set; }
    }
}
