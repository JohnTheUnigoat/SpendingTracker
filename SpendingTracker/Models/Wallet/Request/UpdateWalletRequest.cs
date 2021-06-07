using System.Collections.Generic;

namespace SpendingTracker.Models.Wallet.Request
{
    public class UpdateWalletRequest
    {
        public string Name { get; set; }

        public IEnumerable<string> SharedEmails { get; set; }
    }
}
