using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceLayer.DTO;

namespace ServiceLayer.Services
{
    public interface IIngredientService
    {
        public IngredientDTO GetIngredientById(int ingredientId);
        public IEnumerable<IngredientDTO> GetIngredientsOfTheMeal(int mealId);
    }
}
