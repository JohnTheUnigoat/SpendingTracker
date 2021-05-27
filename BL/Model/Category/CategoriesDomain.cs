using System.Collections.Generic;

namespace BL.Model.Category
{
    public class CategoriesDomain
    {
        public IEnumerable<CategoryDomain> Income { get; set; }

        public IEnumerable<CategoryDomain> Expense { get; set; }
    }
}
