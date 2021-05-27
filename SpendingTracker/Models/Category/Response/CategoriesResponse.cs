using System.Collections.Generic;

namespace SpendingTracker.Models.Category.Response
{
    public class CategoriesResponse
    {
        public IEnumerable<CategoryResponse> Income { get; set; }

        public IEnumerable<CategoryResponse> Expense { get; set; }
    }
}
