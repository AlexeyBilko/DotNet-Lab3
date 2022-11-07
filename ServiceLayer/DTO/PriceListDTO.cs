using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DTO
{
    public class PricelistDTO
    {
        public int Id { get; set; }
        public float Price { get; set; }
        public int MealId { get; set; }
    }
}
