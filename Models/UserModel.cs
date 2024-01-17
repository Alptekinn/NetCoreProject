namespace PetimOl.Models
{
    public class UserModel
    {
        public int UserID { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? LastName { get; set; }
        public string? Sex { get; set; }
        public string? Email { get; set; }
    }
}
