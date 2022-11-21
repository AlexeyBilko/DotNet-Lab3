using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;
using ServiceLayer.DTO;

namespace ServiceLayer.Services
{
    public interface IOrderService : IService<Order, OrderDTO, int>
    {
        public OrderDTO CreateOrder(int tableNumber, DateTime createdTime);
        public void AddMealToOrder(int orderId, int mealId);
        public void RemoveMealFromOrder(int orderId, int mealId);
        public IEnumerable<MealDTO> GetMealsInOder(int orderId);
    }
}
