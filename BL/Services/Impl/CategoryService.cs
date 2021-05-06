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

        public async Task<int> AddCategoryAsync(AddCategoryDto dto)
        {
            var newCategory = dto.ToEntity();

            _dbContext.Categories.Add(newCategory);

            await _dbContext.SaveChangesAsync();

            return newCategory.Id;
        }

        public async Task<IEnumerable<CategoryDomain>> GetCategoriesAsync(int walletId)
        {
            return await _dbContext.Categories
                .Where(c => c.WalletId == walletId)
                .Select(c => new CategoryDomain
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }
    }
}
