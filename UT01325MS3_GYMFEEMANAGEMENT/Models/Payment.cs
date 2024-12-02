using System.ComponentModel.DataAnnotations.Schema;
using UT01325MS3_GYMFEEMANAGEMENT.Enum;

namespace UT01325MS3_GYMFEEMANAGEMENT.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int MemberId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentType PaymentType { get; set; } // "Registration" or "Monthly"

        // Navigation property
        public virtual Member Member { get; set; }
        public DateTime? DueDate { get; set; } // Optional: For future monthly payments

    }
}
