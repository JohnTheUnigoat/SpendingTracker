using BL.Model.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface ICategoryService
    {
        Task<bool> IsUserAuthorizedForCategoryAsync(int categoryId, int userId);

        Task<int> AddCategoryAsync(AddCategoryDto dto);

        Task<CategoriesDomain> GetCategoriesAsync(int userId);

        Task RenameCategory(int categoryId, string name);

        Task DeleteCategory(int categoryId);
    }
}
