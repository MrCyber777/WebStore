
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Models.ViewModel
{
    public class AdminUsersListViewModel
    {
        public List<ApplicationUser> ApplicationUsers { get; set; }
        public PageInfo PaginationInfo { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
    }
}
