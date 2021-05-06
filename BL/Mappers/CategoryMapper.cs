using BL.Model.Category;
using DAL_EF.Entity;

namespace BL.Mappers
{
    public static class CategoryMapper
    {

        public static Category ToEntity(this AddCategoryDto dto) => new Category
        {
            Name = dto.CategoryName,
            WalletId = dto.WalletId
        };
    }
}
