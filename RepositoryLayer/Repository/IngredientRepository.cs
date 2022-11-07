using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace RepositoryLayer.Repository
{
    public class IngredientRepository : GenericRepository<Ingredient>, IIngredientRepository
    {
        public IngredientRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Ingredient> GetIngredientInMeal(int mealId)
        {
            var result = context
                .Ingredients
                .Where(ingredient => ingredient.MealId == mealId)
                .ToList();
            return result;
        }
    }
}
