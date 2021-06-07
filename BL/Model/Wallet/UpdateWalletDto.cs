using System.Collections.Generic;

namespace BL.Model.Wallet
{
    public class UpdateWalletDto
    {
        public int WalletId { get; set; }

        public string Name { get; set; }

        public IEnumerable<string> SharedEmails { get; set; }
    }
}
