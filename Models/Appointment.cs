using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a name")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Please enter a surname")]
        public string CustomerSurname { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerEmail { get; set; }
        [Required(ErrorMessage = "Please enter the first address line")]
        [Display(Name ="Line 1")]
        public string Line1 { get; set; }
        [Required(ErrorMessage ="Please enter  a city name")]
        public string City { get; set; }
        public string Zip { get; set; }
        [Required(ErrorMessage ="Please enter a country name")]
        public string Country { get; set; }
        public DateTime AppointmentDay { get; set; }
        [NotMapped]
        public DateTime AppointmentTime { get; set; }
        public bool IsConfirmed { get; set; }             
        public string SalesPersonID {get; set;}
        [ForeignKey(nameof(SalesPersonID))]
        public virtual ApplicationUser SalesPerson { get; set; }
    }
}
