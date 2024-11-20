namespace UT01325MS3_GYMFEEMANAGEMENT.Entity
{
    public class Member
    {
        public int MemberId { get; set; }
        public string FullName { get; set; }
        public string NIC { get; set; }
        public string ContactDetails { get; set; }
        public DateTime RegistrationDate { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public ICollection<TrainingProgram> TrainingPrograms { get; set; }
    }
}
