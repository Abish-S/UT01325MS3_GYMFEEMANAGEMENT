namespace UT01325MS3_GYMFEEMANAGEMENT.DTOs
{
    public class MemberReportDto
    {
        public int MemberId { get; set; }
        public string FullName { get; set; }
        public string NIC { get; set; }
        public string ContactDetails { get; set; }
        public List<string> TrainingPrograms { get; set; } // Names of training programs
        public decimal TotalPaid { get; set; } // Total amount paid
        public DateTime? LastPaymentDate { get; set; } // Date of the last payment
        public DateTime? NextDueDate { get; set; } // Next payment due date
    }
}
