
using System.Collections.Generic;

namespace WebStore.Models.ViewModel
{
    public class ProductsViewModel
    {
        public Product Products { get; set; }       
        public IEnumerable<ProductTypes> ProductTypes { get; set; }
        public IEnumerable<SpecialTag> SpecialTags { get; set; }
        public PageInfo PaginationInfo { get; set; }




    }
}
