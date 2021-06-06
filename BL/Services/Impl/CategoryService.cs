using BL.Mappers;
using BL.Model.Category;
using DAL_EF;
using DAL_EF.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _dbContext;

        public CategoryService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddCategoryAsync(AddUpdateCategoryDto dto, int userId)
        {
            var newCategory = dto.ToEntity(userId);

            _dbContext.Categories.Add(newCategory);

            await _dbContext.SaveChangesAsync();

            return newCategory.Id;
        }

        private async Task<CategoriesDomain> GetCategoriesDomain(IQueryable<Category> categories)
        {
            List<CategoryDomain> categoryDomains = await categories
                .Select(c => new CategoryDomain
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsIncome = c.IsIncome
                })
                .ToListAsync();

            return new CategoriesDomain
            {
                Income = categoryDomains.Where(c => c.IsIncome).ToList(),
                Expense = categoryDomains.Where(c => c.IsIncome == false).ToList()
            };
        }

        public async Task<CategoriesDomain> GetCategoriesAsync(int userId)
        {
            IQueryable<Category> categories = _dbContext.Categories
                .Where(c => c.UserId == userId);

            return await GetCategoriesDomain(categories);
        }

        public async Task<CategoriesDomain> GetCategoriesForWalletAsync(int walletId)
        {
            IQueryable<int> userIdQueriable = _dbContext.Wallets
                .Where(w => w.Id == walletId)
                .Select(w => w.UserId);

            IQueryable<Category> categories = _dbContext.Categories
                .Join(
                    userIdQueriable,
                    c => c.UserId,
                    uId => uId,
                    (c, uId) => c);

            return await GetCategoriesDomain(categories);
        }

        public async Task DeleteCategory(int categoryId)
        {
            var category = new Category
            {
                Id = categoryId
            };

            _dbContext.Categories.Attach(category);

            _dbContext.Categories.Remove(category);

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCategory(int categoryId, AddUpdateCategoryDto dto)
        {
            Category category = await _dbContext.Categories.FindAsync(categoryId);

            category.Name = dto.CategoryName;
            category.IsIncome = dto.IsIncome;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsUserAuthorizedForCategoryAsync(int categoryId, int userId)
        {
            return await _dbContext.Categories.AnyAsync(c => c.Id == categoryId && c.UserId == userId);
        }
    }
}
