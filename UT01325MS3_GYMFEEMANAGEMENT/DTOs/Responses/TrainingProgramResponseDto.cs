namespace UT01325MS3_GYMFEEMANAGEMENT.DTOs.Responses
{
    public class TrainingProgramResponseDto
    {
        public int TrainingProgramId { get; set; }
        public string ProgramName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; } // Include Price
        public string ImagePath { get; set; }
        public bool Status { get; set; }  // Default to active


    }
}
