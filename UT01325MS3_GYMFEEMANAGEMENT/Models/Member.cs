﻿using System.Text.Json.Serialization;

namespace UT01325MS3_GYMFEEMANAGEMENT.Models
{
    public class Member
    {
        public int MemberId { get; set; }
        public string FullName { get; set; }
        public string NIC { get; set; }
        public string ContactDetails { get; set; }

        // Password properties
        public string PasswordHash { get; set; }

        public DateTime RegistrationDate { get; set; }

        public bool IsAdmin { get; set; } // New property to indicate admin status

        public DateTime? MembershipExpirationDate { get; set; } // Nullable for lifetime memberships

        public bool IsRegistrationFeePaid { get; set; } // Track payment of the initial registration fee

        public virtual ICollection<Payment> Payments { get; set; }
        [JsonIgnore]
        public virtual ICollection<MemberTrainingProgram> MemberTrainingPrograms { get; set; }
        public virtual ICollection<Alert> Alerts { get; set; }
        // Default constructor
        public Member()
        {
            RegistrationDate = DateTime.Today; // Set default value
            Payments = new List<Payment>();
            MemberTrainingPrograms = new List<MemberTrainingProgram>();
            Alerts = new List<Alert>();
        }

    }
}
