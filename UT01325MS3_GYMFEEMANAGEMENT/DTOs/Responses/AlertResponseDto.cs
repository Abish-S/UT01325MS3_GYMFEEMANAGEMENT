namespace UT01325MS3_GYMFEEMANAGEMENT.DTOs.Responses
{
    public class AlertResponseDto
    {
        public int AlertId { get; set; } // Unique ID for the alert
        public int MemberId { get; set; } // ID of the associated member
        public string MemberFullName { get; set; } // Full name of the member
        public string Message { get; set; } // Alert message
        public DateTime CreatedAt { get; set; } // When the alert was created
        public bool IsResolved { get; set; } // Whether the alert has been addressed
    }
}
