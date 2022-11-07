using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace RepositoryLayer.Repository
{
    public interface IMealRepository : IRepository<Meal>
    {
        public IEnumerable<Meal> GetMealsByName(string mealName);
        public IEnumerable<Meal> GetMealsInOrder(int orderId);
    }
}
