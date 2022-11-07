using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DTO;

namespace ServiceLayer.Services
{
    public interface IPricelistService
    {
        public PricelistDTO GetPriceByMealId(int mealId);
    }
}
