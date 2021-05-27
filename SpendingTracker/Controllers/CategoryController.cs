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
    [Route("api/categories")]
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
        public async Task<CategoriesResponse> GetCategories()
        {
            return (await _categoryService.GetCategoriesAsync(UserId)).ToResponse();
        }

        [HttpPost]
        public async Task<int> AddCategory([FromBody] AddUpdateCategoryRequest request)
        {
            return await _categoryService.AddCategoryAsync(request.ToDto(), UserId);
        }

        [HttpPut("{categoryId:int}")]
        public async Task UpdateCategory(int categoryId, [FromBody] AddUpdateCategoryRequest request)
        {
            if(await _categoryService.IsUserAuthorizedForCategoryAsync(categoryId, UserId) == false)
            {
                throw new HttpStatusException(404);
            }

            await _categoryService.UpdateCategory(categoryId, request.ToDto());
        }

        [HttpDelete("{categoryId:int}")]
        public async Task DeleteCategory(int categoryId)
        {
            if (await _categoryService.IsUserAuthorizedForCategoryAsync(categoryId, UserId) == false)
            {
                throw new HttpStatusException(404);
            }

            await _categoryService.DeleteCategory(categoryId);
        }
    }
}
