﻿using BL.Model.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface ICategoryService
    {
        Task<int> AddCategoryAsync(AddCategoryDto dto);

        Task<IEnumerable<CategoryDomain>> GetCategoriesAsync(int walletId);

        Task<bool> IsCategoryInWallet(int categoryId, int walletId);

        Task RenameCategory(int categoryId, string name);

        Task DeleteCategory(int categoryId);
    }
}