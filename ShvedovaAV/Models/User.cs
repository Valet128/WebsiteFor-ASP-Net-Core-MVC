

namespace ShvedovaAV.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; } 
        public string? Email { get; set; }
        public string? Password { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public bool Sendmail { get; set; } = true;
        public Role? Role { get; set; } 
    }
}
