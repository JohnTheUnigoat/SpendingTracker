namespace BL.Model.Category
{
    public class AddCategoryDto
    {
        public string CategoryName { get; set; }

        public bool IsIncome { get; set; }

        public int UserId { get; set; }
    }
}
