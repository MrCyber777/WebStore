
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebStore.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Field name is required")]
        [MinLength(2,ErrorMessage = "Minimum lenght is 2 characters")]
        [MaxLength(2,ErrorMessage = "Maximum length is 30 characters")]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public bool Available { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public string ShadeColor { get; set; }

        public int ProductTypeId { get; set; } // Свойство идентификатора смежной таблицы
        public int SpecialTagId { get; set; }
        [ForeignKey("SpecialTagId")]
        public virtual SpecialTag SpecialTags { get; set; }
        
        [ForeignKey("ProductTypeId")]// Внешний ключ смежной таблицы
        public virtual ProductTypes ProductTypes { get; set; } // Свойство смежной таблицы для хранения информации 
        
    }
}
