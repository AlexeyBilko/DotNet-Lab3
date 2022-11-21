using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace RepositoryLayer.Repository
{
    public class MealRepository : GenericRepository<Meal,int>, IMealRepository
    {
        public MealRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IQueryable<Meal> GetMealsByName(string mealName)
        {
            return context
                .Set<Meal>()
                .Where(meal => meal.Name == mealName);
        }

        public IQueryable<Meal> GetMealsInOrder(int orderId)
        {
            return context
                .MealInOrders
                .Where(mealInOrder => mealInOrder.OrderId == orderId)
                .Join(context.Meals,
                    mealInOrder => mealInOrder.MealId,
                    meal => meal.Id,
                    (mealInOrder, meal) => meal);
        }
    }
}
