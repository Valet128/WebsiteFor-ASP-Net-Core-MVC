
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShvedovaAV.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Password { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
        public bool Sendmail { get; set; } = true;
        public int RoleId { get; set; } = 2;
        public Role? Role { get; set; }
        public List<ContentAccess> ContentAccesses { get; set; } = new(); 
    }
}
