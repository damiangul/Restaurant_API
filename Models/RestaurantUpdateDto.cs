using System.ComponentModel.DataAnnotations;

namespace Restaurant_API.Models
{
    public class RestaurantUpdateDto
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasDelivery { get; set; }
    }
}