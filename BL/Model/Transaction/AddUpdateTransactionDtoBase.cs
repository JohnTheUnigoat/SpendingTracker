﻿using System;

namespace BL.Model.Transaction
{
    public abstract class AddUpdateTransactionDtoBase
    {
        public int WalletId { get; set; }

        public decimal Amount { get; set; }

        public int UserId { get; set; }

        public DateTime? ManualTimestamp { get; set; }
    }
}
