using UT01325MS3_GYMFEEMANAGEMENT.Enum;

namespace UT01325MS3_GYMFEEMANAGEMENT.DTOs
{
    public class PaymentReportDto
    {
        public int PaymentId { get; set; }
        public int MemberId { get; set; }
        public string MemberFullName { get; set; }
        public string MemberContact { get; set; }
        public string PaymentType { get; set; } // Registration, Monthly, etc.
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsOverdue { get; set; } // Whether the payment is overdue
    }
}
