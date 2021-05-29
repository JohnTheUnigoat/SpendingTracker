namespace DAL_EF.Entity
{
    public class WalletAllowedUser
    {
        public int WalletId { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }
    }
}
