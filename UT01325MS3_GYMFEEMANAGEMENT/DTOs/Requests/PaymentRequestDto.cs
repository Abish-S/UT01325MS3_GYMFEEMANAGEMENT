using UT01325MS3_GYMFEEMANAGEMENT.Enum;

namespace UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests
{
    public class PaymentRequestDto
    {
        public int MemberId { get; set; } // Member for whom the payment is made
        public PaymentType PaymentType { get; set; } // Type of payment (e.g., Monthly, Registration)
        public DateTime? PaymentDate { get; set; } // Optional: Date of payment (defaults to current date)
    }
}
