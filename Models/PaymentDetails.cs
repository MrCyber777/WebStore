
using System.ComponentModel.DataAnnotations;
namespace WebStore.Models
{
    public class PaymentDetails
    {
        [Key]
        public int Id { get; set; }
        public double GrossTotal { get; set; }
        public int InvoiceNumber { get; set; }
        public string PaymentStatus { get; set; }
        public string PayerFirstName { get; set; }
        public string PayerLastName { get; set; }
        public string BusinessEmail { get; set; }
        public string PayerEmail { get; set; }
        public string Currency { get; set; }

    }

}
