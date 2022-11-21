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
        //BL remove meal from order 
        // Find meal and remove it from order 
        // read about row version and optimistic concurrency in MSSQL server  [optional]
        public void RemoveMealFromOrder(int mealId, int orderId)
        {
            var mealToRemove = context
                .MealInOrders
                .FirstOrDefault(mealInOrder => mealInOrder.MealId == mealId && mealInOrder.OrderId == orderId);
            if (mealToRemove != null) // Concurency issue [optional]
            {
                context.MealInOrders.Remove(mealToRemove);
            }
        }
    }
}
