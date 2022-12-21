using System.ComponentModel.DataAnnotations;
using ServiceLayer.DTO;

namespace Restaurant.Models
{
    public class MealsViewModel
    {
        [Required(ErrorMessage = "Name")] 
        public string? Name { get; set; }

        [Required(ErrorMessage = "Description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Weight")] 
        public string? Weight { get; set; }
        [Required(ErrorMessage = "Price")] 
        public string? Price { get; set; }

        public List<string>? ingredients { get; set; }
    }
}
