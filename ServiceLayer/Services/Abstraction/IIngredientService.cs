using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;
using ServiceLayer.DTO;

namespace ServiceLayer.Services
{
    public interface IIngredientService : IService<Ingredient, IngredientDTO,int>
    {
        public IEnumerable<IngredientDTO> GetIngredientsOfTheMeal(int mealId);
    }
}
