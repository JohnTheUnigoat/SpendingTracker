using BL.Services;
using DAL_EF;
using DAL_EF.Entity.Transaction;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace SpendingTracker.Controllers
{
    [Route("api/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        private readonly AppDbContext dbContext;

        public TestController(ICategoryService categoryService, AppDbContext dbContext)
        {
            _categoryService = categoryService;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult> GetTest()
        {
            //dbContext.Transactions.Add(new CategoryTransaction
            //{
            //    Amount = 420.00m,
            //    SourceWalletId = 5,
            //    CategoryId = 15
            //});

            //dbContext.Transactions.Add(new WalletTransaction
            //{
            //    Amount = 420.00m,
            //    SourceWalletId = 5,
            //    TargetWalletId = 2
            //});

            //await dbContext.SaveChangesAsync();

            //return Ok();
            //return Ok(await _categoryService.GetCategoriesAsync(walletId));

            var res = dbContext.Transactions.Select(t => new Test
            {
                TransactionId = t.Id,
                Amount = t.Amount,
                Target = (t is CategoryTransaction) ?
                    (t as CategoryTransaction).Category.Name :
                    (t as WalletTransaction).TargetWallet.Name
            });

            return Ok(res);
        }

        //[HttpPost]
        //public async Task<ActionResult> PostTest([FromBody] TestPost model)
        //{
        //    return Ok(await _categoryService.AddCategoryAsync(model.CategoryName, model.WalletId));
        //}
    }

    public class Test
    {
        public int TransactionId { get; set; }

        public string Target { get; set; }

        public decimal Amount { get; set; }
    }

    public class TestPost
    {
        public string CategoryName { get; set; }

        public int WalletId { get; set; }
    }
}
