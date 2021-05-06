using BL.Model.Category;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Services
{
    public interface ICategoryService
    {
        Task<int> AddCategoryAsync(string name, int walletId);

        Task<IEnumerable<CategoryDomain>> GetCategoriesAsync(int walletId);
    }
}
