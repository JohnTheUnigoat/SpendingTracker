using BL.Model.Category;
using DAL_EF.Entity;

namespace BL.Mappers
{
    public static class CategoryMapper
    {

        public static Category ToEntity(this AddUpdateCategoryDto dto, int userId) => new Category
        {
            Name = dto.CategoryName,
            IsIncome = dto.IsIncome,
            UserId = userId
        };
    }
}
