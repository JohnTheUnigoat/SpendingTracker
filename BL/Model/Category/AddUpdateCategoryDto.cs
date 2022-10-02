namespace BL.Model.Category
{
    public class AddUpdateCategoryDto
    {
        public string CategoryName { get; set; }

        public int WalletId { get; set; }
        
        public bool IsIncome { get; set; }
    }
}
