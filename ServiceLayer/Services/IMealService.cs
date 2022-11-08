using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;
using ServiceLayer.DTO;

namespace ServiceLayer.Services
{
    public interface IMealService : IService<Meal, MealDTO>
    {
        public IEnumerable<MealDTO> GetMealsByName(string mealName);
        public MealDTO GetMealById(int mealId);
    }
}
