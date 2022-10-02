using BL.Model.Category;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface ICategoryService
    {
        Task<int> AddCategoryAsync(AddUpdateCategoryDto dto);

        Task<CategoriesDomain> GetCategories(int walletId);

        Task UpdateCategory(int categoryId, AddUpdateCategoryDto dto);

        Task DeleteCategory(int categoryId);
    }
}
