using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace RepositoryLayer.Repository
{
    public class OrderRepository : GenericRepository<Order,int>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Order> GetOrdersByTable(int tableNumber)
        {
            var result = context
                .Orders
                .Where(order => order.TableNumber == tableNumber);

            return result;
        }

        public bool DeleteId(int orderId)
        {
            var mealinorders = context.MealInOrders.Where(order => order.OrderId == orderId);
            context.MealInOrders.RemoveRange(mealinorders);
            var order = context.Orders.SingleOrDefault(x => x.Id == orderId);
            context.Orders.Remove(order);
            context.SaveChanges();
            return true;
        }
    }
}
