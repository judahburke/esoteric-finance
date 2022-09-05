using System.Collections.Generic;

namespace Esoteric.Finance.Abstractions.DataTransfer.Categories
{
    public class CategoryResponse : CategoryModel
    {
        public int? Id { get; set; }
        public IList<SubCategoryResponse> SubCategories { get; set; }
    }
}
