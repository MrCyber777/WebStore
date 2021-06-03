using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStore.Models
{
    public class ProductsForAppointment
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        [ForeignKey("UserId")]
        public virtual ClientUser User { get; set; }

        [ForeignKey("AppointmentId")]
        public virtual Appointment Appointments { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Products { get; set; }
    }
}