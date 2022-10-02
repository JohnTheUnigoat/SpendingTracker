using System.Collections.Generic;

namespace DAL_EF.Entity
{
    public class Wallet
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public string DefaultReportPeriod { get; set; }

        public List<WalletAllowedUser> WalletAllowedUsers { get; set; }
    }
}
