using ServiceLayer.DTO;

namespace Restaurant.Models
{
    public class OrdersModel
    {
        public int OrderId { get; set; }
        public int TableNumber { get; set; }
        public DateTime OrderedTime { get; set; }
        public List<MealDTO> mealsInOrder { get; set; }
    }
}
