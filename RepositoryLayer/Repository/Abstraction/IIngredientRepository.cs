using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace RepositoryLayer.Repository
{
    public interface IIngredientRepository : IRepository<Ingredient,int>
    {
        public IEnumerable<Ingredient> GetIngredientInMeal(int mealId);
    }
}
