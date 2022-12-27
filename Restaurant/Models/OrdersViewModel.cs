using System.ComponentModel.DataAnnotations;
using ServiceLayer.DTO;

namespace Restaurant.Models
{
    public class OrdersViewModel
    {
        [Required(ErrorMessage = "TableNumber")]
        public int TableNumber { get; set; }

        [Required(ErrorMessage = "OrderedTime")]
        public DateTime OrderedTime { get; set; }

        //public List<MealDTO>? meals { get; set; }
        public List<string>? meals { get; set; }
    }
}
