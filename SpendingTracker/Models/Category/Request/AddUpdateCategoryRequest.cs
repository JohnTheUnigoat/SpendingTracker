using Microsoft.AspNetCore.Mvc;

namespace SpendingTracker.Models.Category.Request
{
    public class AddUpdateCategoryRequest
    {
        public string Name { get; set; }

        public bool IsIncome { get; set; }
    }
}
