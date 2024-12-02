using UT01325MS3_GYMFEEMANAGEMENT.Enum;

namespace UT01325MS3_GYMFEEMANAGEMENT.DTOs
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentType PaymentType { get; set; }
    }
}
