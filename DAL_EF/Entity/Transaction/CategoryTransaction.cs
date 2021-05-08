namespace DAL_EF.Entity.Transaction
{
    public class CategoryTransaction : TransactionBase
    {
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
