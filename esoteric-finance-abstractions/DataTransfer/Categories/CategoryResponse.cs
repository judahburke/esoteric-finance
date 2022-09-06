using Esoteric.Finance.Abstractions.DataTransfer.Details;
using System;
using System.Collections.Generic;
using System.Text;

namespace Esoteric.Finance.Abstractions.DataTransfer.Categories
{
    public class CategoryResponse : CommonNamedEntityModel
    {
        public virtual int Id { get; set; }
        public virtual IEnumerable<DetailResponse> Details { get; set; }
    }
}
