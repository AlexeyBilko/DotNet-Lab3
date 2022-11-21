using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace RepositoryLayer.Repository
{
    public interface IMealInOrderRepository : IRepository<MealInOrder, int>
    {
        public void FindMealAndRemoveFromOrder(Meal meal, int orderId);
    }
}
