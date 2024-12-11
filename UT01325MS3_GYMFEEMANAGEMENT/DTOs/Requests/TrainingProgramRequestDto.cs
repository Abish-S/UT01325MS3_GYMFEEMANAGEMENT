namespace UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests
{
    public class TrainingProgramRequestDto
    {
        public string ProgramName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; } // Include Price
        public string Base64Image { get; set; } // Base64 encoded image

        public bool Status { get; set; } = true; // Default to active




    }
}
