using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace RepositoryLayer.Repository
{
    public class MealRepository : GenericRepository<Meal>, IMealRepository
    {
        public MealRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Meal> GetMealsByName(string mealName)
        {
            var result = context
                .Set<Meal>()
                .Where(meal => meal.Name == mealName)
                .ToList();
            return result;
        }

        public IEnumerable<Meal> GetMealsInOrder(int orderId)
        {
            var result = context
                .MealInOrders
                .Where(mealInOrder => mealInOrder.OrderId == orderId)
                .Join(context.Meals,
                    mealInOrder => mealInOrder.MealId,
                    meal => meal.Id,
                    (mealInOrder, meal) => meal)
                .ToList();
            return result;
        }
    }
}
