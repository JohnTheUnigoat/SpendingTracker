﻿using BL.Mappers;
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

        public async Task<CategoriesDomain> GetCategoriesAsync(int userId)
        {
            List<CategoryDomain> categories = await _dbContext.Categories
                .Where(c => c.UserId == userId)
                .Select(c => new CategoryDomain
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsIncome = c.IsIncome
                })
                .ToListAsync();

            return new CategoriesDomain
            {
                Income = categories.Where(c => c.IsIncome).ToList(),
                Expense = categories.Where(c => c.IsIncome == false).ToList()
            };
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

        public async Task RenameCategory(int categoryId, string name)
        {
            Category category = await _dbContext.Categories.FindAsync(categoryId);

            category.Name = name;

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsUserAuthorizedForCategoryAsync(int categoryId, int userId)
        {
            return await _dbContext.Categories.AnyAsync(c => c.Id == categoryId && c.UserId == userId);
        }
    }
}
