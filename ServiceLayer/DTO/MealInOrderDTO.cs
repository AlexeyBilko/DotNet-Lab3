using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.DTO
{
    public class MealInOrderDTO
    {
        public int Id { get; set; }
        public int MealId { get; set; }
        public int OrderId { get; set; }
    }
}
