
using System.Collections.Generic;

namespace WebStore.Models.ViewModel
{
    public class ShoppingCartViewModel
    {
        public List<Product> Products { get; set; }
        public Appointment Appointment { get; set; }
        public PageInfo PaginationInfo { get; set; }
    }
}
