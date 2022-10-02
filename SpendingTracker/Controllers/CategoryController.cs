using BL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Mappers;
using SpendingTracker.Models.Category.Request;
using SpendingTracker.Models.Category.Response;
using System.Threading.Tasks;

namespace SpendingTracker.Controllers
{
    [Route("api/wallets/{walletId:int}/categories")]
    [ApiController]
    [Authorize]
    public class CategoryController : BaseController
    {
        // TODO: centralized wallet authorization
        private readonly ICategoryService _categoryService;
        private readonly IWalletService _walletService;

        public CategoryController(ICategoryService categoryService, IWalletService walletService)
        {
            _categoryService = categoryService;
            _walletService = walletService;
        }

        [HttpGet]
        public async Task<CategoriesResponse> GetCategories(int walletId)
        {
            return (await _categoryService.GetCategories(walletId)).ToResponse();
        }

        [HttpPost]
        public async Task<int> AddCategory(int walletId, AddUpdateCategoryRequest request)
        {
            return await _categoryService.AddCategoryAsync(request.ToDto(walletId));
        }
        
        [HttpPut("{categoryId:int}")]
        public async Task UpdateCategory(int walletId, int categoryId, AddUpdateCategoryRequest request)
        {
            await _categoryService.UpdateCategory(categoryId, request.ToDto(walletId));
        }

        [HttpDelete("{categoryId:int}")]
        public async Task DeleteCategory(int categoryId)
        {
            await _categoryService.DeleteCategory(categoryId);
        }
    }
}
