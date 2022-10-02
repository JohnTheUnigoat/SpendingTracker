using BL.Model.Category;
using SpendingTracker.Models.Category.Request;
using SpendingTracker.Models.Category.Response;
using System.Collections.Generic;
using System.Linq;

namespace SpendingTracker.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryResponse ToResponse(this CategoryDomain domain) => new CategoryResponse
        {
            Id = domain.Id,
            Name = domain.Name
        };

        public static IEnumerable<CategoryResponse> AllToResponse(
            this IEnumerable<CategoryDomain> domains) => domains
                .Select(d => d.ToResponse())
                .ToList();

        public static CategoriesResponse ToResponse(this CategoriesDomain domain) => new CategoriesResponse
        {
            Income = domain.Income.AllToResponse(),
            Expense = domain.Expense.AllToResponse()
        };

        public static AddUpdateCategoryDto ToDto(this AddUpdateCategoryRequest request, int walletId) => new()
        {
            CategoryName = request.Name,
            WalletId = walletId,
            IsIncome = request.IsIncome,
        };
    }
}
