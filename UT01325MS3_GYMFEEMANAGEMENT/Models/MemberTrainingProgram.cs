namespace UT01325MS3_GYMFEEMANAGEMENT.Models
{
    public class MemberTrainingProgram
    {
        public int MemberId { get; set; }
        public virtual Member Member { get; set; }

        public int TrainingProgramId { get; set; }
        public virtual TrainingProgram TrainingProgram { get; set; }
    }
}
