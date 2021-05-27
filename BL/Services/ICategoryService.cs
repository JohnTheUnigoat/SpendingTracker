using BL.Model.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface ICategoryService
    {
        Task<bool> IsUserAuthorizedForCategoryAsync(int categoryId, int userId);

        Task<int> AddCategoryAsync(AddUpdateCategoryDto dto, int userId);

        Task<CategoriesDomain> GetCategoriesAsync(int userId);

        Task UpdateCategory(int categoryId, AddUpdateCategoryDto dto);

        Task DeleteCategory(int categoryId);
    }
}
