using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WebStore.Models
{
    public class ClientUser:IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(100)]
        public string Address { get; set; }
        public string IP { get; set; }
        public string UserMac { get; set; }
    }
}
