using BL.Services;
using Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SpendingTracker.Mappers;
using SpendingTracker.Models.Category.Request;
using SpendingTracker.Models.Category.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            if (!await _walletService.IsUserAuthorizedForWallet(walletId, UserId))
            {
                throw new HttpStatusException(401);
            }

            return (await _categoryService.GetCategoriesAsync(walletId)).AllToResponse();
        }

        [HttpPost]
        public async Task<int> AddCategory([FromRoute] AddCategoryRequest request)
        {
            if (!await _walletService.IsUserAuthorizedForWallet(request.walletId, UserId))
            {
                throw new HttpStatusException(401);
            }

            return await _categoryService.AddCategoryAsync(request.ToDto());
        }
    }
}
