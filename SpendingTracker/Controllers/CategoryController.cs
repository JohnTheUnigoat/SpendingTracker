using BL.Services;
using Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Mappers;
using SpendingTracker.Models.Category.Request;
using SpendingTracker.Models.Category.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpendingTracker.Controllers
{
    [Route("api/wallets/{walletId:int}/categories")]
    [ApiController]
    [Authorize]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IWalletService _walletService;

        public CategoryController(ICategoryService categoryService, IWalletService walletService)
        {
            _categoryService = categoryService;
            _walletService = walletService;
        }

        [HttpGet]
        public async Task<IEnumerable<CategoryResponse>> GetCategories(int walletId)
        {
            return (await _categoryService.GetCategoriesAsync(walletId)).AllToResponse();
        }

        [HttpPost]
        public async Task<int> AddCategory(int walletId, [FromBody] AddUpdateCategoryRequest request)
        {
            return await _categoryService.AddCategoryAsync(request.ToDto(walletId));
        }

        [HttpPut("{categoryId:int}")]
        public async Task RenameCategory(int walletId, int categoryId, [FromBody] AddUpdateCategoryRequest request)
        {
            if(await _categoryService.IsCategoryInWallet(categoryId, walletId) == false)
            {
                throw new HttpStatusException(404);
            }

            await _categoryService.RenameCategory(categoryId, request.Name);
        }

        [HttpDelete("{categoryId:int}")]
        public async Task DeleteCategory(int walletId, int categoryId)
        {
            if (await _categoryService.IsCategoryInWallet(categoryId, walletId) == false)
            {
                throw new HttpStatusException(404);
            }

            await _categoryService.DeleteCategory(categoryId);
        }
    }
}
