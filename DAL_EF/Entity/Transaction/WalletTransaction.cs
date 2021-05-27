namespace DAL_EF.Entity.Transaction
{
    public class WalletTransaction : TransactionBase
    {
        public int OtherWalletId { get; set; }

        public Wallet OtherWallet { get; set; }
    }
}
