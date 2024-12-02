namespace UT01325MS3_GYMFEEMANAGEMENT.DTOs.Responses
{
    public class StandardErrorResponse
    {
        public int StatusCode { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
        public string Detail { get; set; }
        public string Instance { get; set; }  
    }
}
