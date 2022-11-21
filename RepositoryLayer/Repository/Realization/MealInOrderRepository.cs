using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace RepositoryLayer.Repository
{
    public class MealInOrderRepository : GenericRepository<MealInOrder, int>, IMealInOrderRepository
    {
        public MealInOrderRepository(ApplicationDbContext context) : base(context)
        {
        }
        // read about row version and optimistic concurrency in MSSQL server  [optional]
        public void FindMealAndRemoveFromOrder(Meal meal, int orderId)
        {
            var mealToRemove = context
                .MealInOrders
                .FirstOrDefault(mealInOrder => mealInOrder.MealId == meal.Id && mealInOrder.OrderId == orderId);
            if (mealToRemove != null) // Concurency issue [optional]
            {
                context.MealInOrders.Remove(mealToRemove);
            }
        }
    }
}
