namespace BL.Model.Transaction
{
    public class AddUpdateWalletTransactionDto : AddUpdateTransactionDtoBase
    {
        public int TargetWalletId { get; set; }
    }
}
