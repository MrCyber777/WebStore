using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool Available { get; set; }
        public string Image { get; set; }
        public string ShadeColor { get; set; }

        public int ProductTypeId { get; set; } // Свойство идентификатора смежной таблицы
        public int SpecialTagId { get; set; }
        [ForeignKey("SpecialTagId")]
        public virtual SpecialTag SpecialTags { get; set; }
        
        [ForeignKey("ProductTypeId")]// Внешний ключ смежной таблицы
        public virtual ProductTypes ProductTypes { get; set; } // Свойство смежной таблицы для хранения информации 
        
    }
}
