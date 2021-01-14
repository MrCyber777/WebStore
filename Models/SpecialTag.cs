using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    public class SpecialTag
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Field bestSellar is required")]
        [MinLength(2, ErrorMessage = "Minimum lenght is 2 characters")]
        [MaxLength(30, ErrorMessage = "Maximum length is 30 characters")]
        public string bestSeller { get; set; }
        [Required(ErrorMessage = "Field newEdition is required")]
        [MinLength(2, ErrorMessage = "Minimum lenght is 2 characters")]
        [MaxLength(30, ErrorMessage = "Maximum length is 30 characters")]
        public string newEdition { get; set; }
        [Required(ErrorMessage = "Field specialSale is required")]
        [MinLength(2, ErrorMessage = "Minimum lenght is 2 characters")]
        [MaxLength(30, ErrorMessage = "Maximum length is 30 characters")]
        public string specialSale { get; set; }
    }
}
