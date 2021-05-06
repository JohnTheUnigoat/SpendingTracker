using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL_EF.Entity
{
    public class UserSettings
    {
        [Key]
        public int UserId { get; set; }

        public ICollection<Wallet> Wallets { get; set; }
    }
}
