using Microsoft.AspNetCore.Mvc;

namespace SpendingTracker.Models.Category.Request
{
    public class AddCategoryRequest
    {
        public int walletId { get; set; }

        [FromBody]
        public string CategoryName { get; set; }
    }
}
