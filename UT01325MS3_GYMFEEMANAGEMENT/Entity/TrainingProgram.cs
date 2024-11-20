namespace UT01325MS3_GYMFEEMANAGEMENT.Entity
{
    public class TrainingProgram
    {
        public int TrainingProgramId { get; set; }
        public string ProgramName { get; set; }
        public string Description { get; set; }
        public ICollection<Member> EnrolledMembers { get; set; }
    }
}
