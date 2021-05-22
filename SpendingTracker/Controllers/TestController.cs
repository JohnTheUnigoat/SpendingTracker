using BL.Model.Transaction;
using BL.Services;
using Core.Const;
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
        private readonly ITransactionService _transactionService;

        private readonly AppDbContext dbContext;

        public TestController(ICategoryService categoryService, AppDbContext dbContext, ITransactionService transactionService)
        {
            _categoryService = categoryService;
            this.dbContext = dbContext;
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<ActionResult> GetTest(int walletId)
        {
            var res = await _transactionService.GetTransactionsAsync(new GetTransactionsDto
            {
                WalletId = walletId,
                ReportPeriod = ReportPeriods.CurrentYear
            });

            //var res = await _transactionService.AddTransactionAsync(new AddCategoryTransactionDto
            //{
            //    Amount = -420.69m,
            //    CaterodyId = 15,
            //    WalletId = 5
            //});

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
