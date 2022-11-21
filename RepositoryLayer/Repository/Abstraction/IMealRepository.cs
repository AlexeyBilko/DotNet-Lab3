using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace RepositoryLayer.Repository
{
    public interface IMealRepository : IRepository<Meal, int>
    {
        public IQueryable<Meal> GetMealsByName(string mealName);
        public IQueryable<Meal> GetMealsInOrder(int orderId);
    }
}
