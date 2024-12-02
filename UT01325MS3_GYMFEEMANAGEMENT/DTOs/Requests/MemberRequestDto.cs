namespace UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests
{
    public class MemberRequestDto
    {
        public string FullName { get; set; }
        public string NIC { get; set; }
        public string ContactDetails { get; set; }
        public DateTime RegistrationDate { get; set; }
        public PaymentRequestDto Payment { get; set; }

        public string Password { get; set; }

        public List<int> SelectedTrainingProgramIds { get; set; }
        public MemberRequestDto()
        {
            SelectedTrainingProgramIds = new List<int>();
        }

    }
}
