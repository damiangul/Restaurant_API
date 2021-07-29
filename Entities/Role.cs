using System.ComponentModel.DataAnnotations;

namespace Restaurant_API.Entities
{
    public class Role
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}