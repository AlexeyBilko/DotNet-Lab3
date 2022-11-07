using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DTO;

namespace ServiceLayer.Services
{
    public interface IOrderService
    {
        public OrderDTO CreateOrder(int tableNumber, DateTime createdTime);
        public void AddMealToOrder(int orderId, int mealId);
        public void RemoveMealFromOrder(int orderId, int mealId);
        public IEnumerable<MealDTO> GetMealsInOder(int orderId);
        public OrderDTO GetOrderById(int orderId);
    }
}
