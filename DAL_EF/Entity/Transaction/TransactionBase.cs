using System;

namespace DAL_EF.Entity.Transaction
{
    public abstract class TransactionBase
    {
        public int Id { get; set; }

        public int WalletId { get; set; }

        public Wallet Wallet { get; set; }

        public decimal Amount { get; set; }

        public DateTime TimeStamp { get; set; }
    }
}
