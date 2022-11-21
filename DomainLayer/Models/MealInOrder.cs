using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class MealInOrder : BaseEntity<int>
    {
        public int MealId { get; set; }
        public Meal Meal { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }

    }
}
