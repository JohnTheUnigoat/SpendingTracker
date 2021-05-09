namespace BL.Model.Transaction
{
    public class AddWalletTransactionDto : AddTransactionDtoBase
    {
        public int TargetWalletId { get; set; }
    }
}
