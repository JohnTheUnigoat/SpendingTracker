using System;

#nullable enable

namespace Core.Exceptions.CustomExceptions
{
    public class AttemptedChangeOfTransactionTypeException : CustomExceptionBase
    {
        public override ErrorCode ErrorCode { get => ErrorCode.AttemptedChangeOfTransactionType; }

        public AttemptedChangeOfTransactionTypeException() : 
            base("Changing the type of an existing transaction's target (i.e. category -> wallet) is not supported")
        {
        }
    }
}
