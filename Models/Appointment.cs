using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int CustomerPhoneNumber { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime AppointmentDay { get; set; }
        [NotMapped]
        public DateTime AppointmentTime { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
