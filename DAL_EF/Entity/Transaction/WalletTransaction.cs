namespace DAL_EF.Entity.Transaction
{
    public class WalletTransaction : TransactionBase
    {
        public int SourceWalletId { get; set; }

        public Wallet SourceWallet { get; set; }
    }
}
