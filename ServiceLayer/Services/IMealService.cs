using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DTO;

namespace ServiceLayer.Services
{
    public interface IMealService
    {
        public IEnumerable<MealDTO> GetMealsByName(string mealName);
        public MealDTO GetMealById(int mealId);
    }
}
