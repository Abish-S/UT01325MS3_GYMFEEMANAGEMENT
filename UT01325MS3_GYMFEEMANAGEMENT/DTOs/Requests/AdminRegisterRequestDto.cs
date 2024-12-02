namespace UT01325MS3_GYMFEEMANAGEMENT.DTOs.Requests
{
    public class AdminRegisterRequestDto
    {
        public string FullName { get; set; }
        public string NIC { get; set; }
        public string ContactDetails { get; set; }
        public DateTime RegistrationDate { get; set; }

        public string Password { get; set; }

    }
}
