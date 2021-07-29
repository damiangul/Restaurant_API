using System.ComponentModel.DataAnnotations;

namespace Restaurant_API.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public System.DateTime? DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}