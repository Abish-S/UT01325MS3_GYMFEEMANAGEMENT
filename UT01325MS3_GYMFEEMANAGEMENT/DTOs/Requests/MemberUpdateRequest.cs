namespace UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests
{
    public class MemberUpdateRequest
    {
        public string FullName { get; set; }
       
        public string ContactDetails { get; set; }

        public string Password { get; set; }

        public List<int> SelectedTrainingProgramIds { get; set; }
        public MemberUpdateRequest()
        {
            SelectedTrainingProgramIds = new List<int>();
        }

    
}
}
