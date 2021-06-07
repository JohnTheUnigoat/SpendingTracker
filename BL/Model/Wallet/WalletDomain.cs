using System.Collections.Generic;

namespace BL.Model.Wallet
{
    public class WalletDomain
    {
        public int Id { get; set; }

        public string OwnerEmail { get; set; }

        public List<string> SharedEmails { get; set; }

        public string Name { get; set; }

        public string DefaultReportPeriod { get; set; }
    }
}
