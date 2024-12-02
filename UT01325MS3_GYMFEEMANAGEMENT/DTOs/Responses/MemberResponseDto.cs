namespace UT01325MS3_GYMFEEMANAGEMENT.DTOs.Responses
{
    public class MemberResponseDto
    {
        public int MemberId { get; set; }
        public string FullName { get; set; }
        public string NIC { get; set; }
        public string ContactDetails { get; set; }
        public DateTime RegistrationDate { get; set; }

      
        public bool IsRegistrationFeePaid { get; set; }

        // List of associated training programs
        public List<TrainingProgramResponseDto> SelectedTrainingPrograms { get; set; }
    }
}
