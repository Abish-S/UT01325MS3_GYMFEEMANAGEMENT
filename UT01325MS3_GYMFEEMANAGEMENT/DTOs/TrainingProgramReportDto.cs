namespace UT01325MS3_GYMFEEMANAGEMENT.DTOs
{
    public class TrainingProgramReportDto
    {
        public int TrainingProgramId { get; set; }
        public string ProgramName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int TotalMembers { get; set; } // Number of enrolled members
        public List<MemberEnrollmentDto> EnrolledMembers { get; set; } // Details of enrolled members
    }
}
