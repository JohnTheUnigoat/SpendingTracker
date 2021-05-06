using BL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SpendingTracker.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public TestController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult> GetTest(int walletId)
        {
            return Ok(await _categoryService.GetCategoriesAsync(walletId));
        }

        //[HttpPost]
        //public async Task<ActionResult> PostTest([FromBody] TestPost model)
        //{
        //    return Ok(await _categoryService.AddCategoryAsync(model.CategoryName, model.WalletId));
        //}
    }

    public class TestPost
    {
        public string CategoryName { get; set; }

        public int WalletId { get; set; }
    }
}
