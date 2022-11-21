using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace RepositoryLayer.Repository
{
    public class PricelistRepository : GenericRepository<Pricelist,int>, IPricelistRepository
    {
        public PricelistRepository(ApplicationDbContext context) : base(context)
        {
        }
        public Pricelist GetPriceByMeal(int mealId)
        {
            var result = context
                .Pricelist
                .Where(pricelist => pricelist.MealId == mealId);
            return result.First();
        }
    }
}
