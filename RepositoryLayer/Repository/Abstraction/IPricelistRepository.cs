using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace RepositoryLayer.Repository
{
    public interface IPricelistRepository : IRepository<Pricelist,int>
    {
        public Pricelist GetPriceByMeal(int mealId);
    }
}
