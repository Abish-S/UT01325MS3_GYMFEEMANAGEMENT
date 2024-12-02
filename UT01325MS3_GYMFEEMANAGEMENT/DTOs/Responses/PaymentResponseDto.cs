using UT01325MS3_GYMFEEMANAGEMENT.Enum;

namespace UT01325MS3_GYMFEEMANAGEMENT.DTOs.Responses
{
    public class PaymentResponseDto
    {
        public int PaymentId { get; set; } // ID of the payment
        public int MemberId { get; set; } // Member associated with the payment
        public string MemberFullName { get; set; } // New property

        public decimal Amount { get; set; } // Payment amount
        public PaymentType PaymentType { get; set; } // Type of payment (e.g., Monthly)
        public DateTime PaymentDate { get; set; } // Date of payment
        public DateTime? DueDate { get; set; } // Optional: Next due date
    }
}
