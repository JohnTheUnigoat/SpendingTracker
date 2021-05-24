namespace BL.Model.Transaction
{
    public class AddUpdateWalletTransactionDto : AddUpdateTransactionDtoBase
    {
        public int SourceWalletId { get; set; }
    }
}
