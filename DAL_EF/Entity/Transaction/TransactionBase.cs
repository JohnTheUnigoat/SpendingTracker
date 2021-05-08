namespace DAL_EF.Entity.Transaction
{
    public abstract class TransactionBase
    {
        public int Id { get; set; }

        public int SourceWalletId { get; set; }

        public Wallet SourceWallet { get; set; }

        public decimal Amount { get; set; }
    }
}
