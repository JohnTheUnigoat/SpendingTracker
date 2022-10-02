namespace DAL_EF.Entity
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsIncome { get; set; }

        public int WalletId { get; set; }
    }
}
