namespace DAL_EF.Entity.Transaction
{
    public class WalletTransaction : TransactionBase
    {
        public int TargetWalletId { get; set; }

        public Wallet TargetWallet { get; set; }
    }
}
