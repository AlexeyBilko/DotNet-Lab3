using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
    public class Pricelist : BaseEntity<int>
    {
        public float Price { get; set; }
        public int MealId { get; set; }
        public Meal Meal { get; set; }
    }
}
