using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;
using ServiceLayer.DTO;

namespace ServiceLayer.Services
{
    public interface IPricelistService : IService<Pricelist, PricelistDTO>
    {
        public PricelistDTO GetPriceByMealId(int mealId);
    }
}
