namespace Esoteric.Finance.Abstractions.DataTransfer.Categories
{
    public class SubCategoryRequest : SubCategoryModel
    {
        public virtual int CategoryId { get; set; }
        public virtual int? Id { get; set; }
    }
}
