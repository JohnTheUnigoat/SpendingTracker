using Microsoft.AspNetCore.Mvc;

namespace SpendingTracker.Models.Category.Request
{
    public class AddUpdateCategoryRequest
    {

        [FromBody]
        public string Name { get; set; }
    }
}
