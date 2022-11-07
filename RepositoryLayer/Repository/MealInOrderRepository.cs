using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace RepositoryLayer.Repository
{
    public class MealInOrderRepository : GenericRepository<MealInOrder>, IMealInOrderRepository
    {
        public MealInOrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void RemoveMealFromOrder(int mealId, int orderId)
        {
            var mealToRemove = context
                .MealInOrders
                .FirstOrDefault(mealInOrder => mealInOrder.MealId == mealId & mealInOrder.OrderId == orderId);
            context.MealInOrders.Remove(mealToRemove);
        }
    }
}
