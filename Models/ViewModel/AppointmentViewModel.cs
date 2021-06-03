using System.Collections.Generic;

namespace WebStore.Models.ViewModel
{
    public class AppointmentViewModel
    {
        public List<Appointment> Appointments { get; set; }
        public PageInfo PaginationInfo { get; set; }
    }
}