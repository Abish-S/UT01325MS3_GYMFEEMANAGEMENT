namespace UT01325MS3_GYMFEEMANAGEMENT.Models
{
    public class TrainingProgram
    {
        public int TrainingProgramId { get; set; }
        public string ProgramName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; } // Added Price property
        public virtual ICollection<MemberTrainingProgram> MemberTrainingPrograms { get; set; }


        // Navigation properties
        public TrainingProgram()
        {
            MemberTrainingPrograms = new List<MemberTrainingProgram>();
        }
    }
}
