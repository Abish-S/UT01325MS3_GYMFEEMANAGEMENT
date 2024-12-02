
namespace UT01325MS3_GYMFEEMANAGEMENT.Models
{
    public class Alert
    {
        public int AlertId { get; set; } // Unique ID for the alert
        public int MemberId { get; set; } // ID of the associated member
        public virtual Member Member { get; set; } // Navigation property for the associated member
        public string Message { get; set; } // Alert message (e.g., "Overdue payment")
        public DateTime CreatedAt { get; set; } // Timestamp of when the alert was created
        public bool IsResolved { get; set; } // Whether the alert has been addressed

        public Alert()
        {
            CreatedAt = DateTime.Now;
            IsResolved = false;
        }
    }
}
