namespace UT01325MS3_GYMFEEMANAGEMENT.Entity
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int MemberId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentType { get; set; }
    }
}
