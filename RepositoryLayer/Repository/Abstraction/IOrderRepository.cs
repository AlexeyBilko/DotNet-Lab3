using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace RepositoryLayer.Repository
{
    public interface IOrderRepository : IRepository<Order, int>
    {
        public IEnumerable<Order> GetOrdersByTable(int tableNumber);
        public bool DeleteId(int orderId);
    }
}
