using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;
using ServiceLayer.DTO;

namespace ServiceLayer.Services
{
    public interface IMealService : IService<Meal, MealDTO, int>
    {
        public IEnumerable<MealDTO> GetMealsByName(string mealName);
    }
}
