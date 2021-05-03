
using System.Collections.Generic;

namespace WebStore.Models.ViewModel
{
    public class AppointmentDetailsViewModel
    {
        public Appointment Appointment { get; set; }
        public List<Product> Products { get; set; }
        public List<ApplicationUser> SalesPersons { get; set; }
       
    }
}
