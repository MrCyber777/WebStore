using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Models.ViewModel
{
    public class ProductListViewModel
    {
        public List<Product> Products { get; set; }
        public PageInfo PaginationInfo { get; set; }
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public bool Available { get; set; }

        public int ProductTypeId { get; set; } // Свойство идентификатора смежной таблицы
        public int SpecialTagId { get; set; }

        [ForeignKey("SpecialTagId")]
        public virtual SpecialTag SpecialTags { get; set; }

        [ForeignKey("ProductTypeId")]// Внешний ключ смежной таблицы
        public virtual ProductTypes ProductTypes { get; set; }
    }
}