using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class Ingredient : BaseEntity
    {
        public string Name { get; set; }
        public float Weight { get; set; }
        public int MealId { get; set; }
        public Meal Meal { get; set; }
    }
}
