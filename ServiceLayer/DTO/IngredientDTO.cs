using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DTO
{
    public class IngredientDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
        public int MealId { get; set; }
    }
}
