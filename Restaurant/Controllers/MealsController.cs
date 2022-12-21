using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.UnitOfWork;
using Restaurant.Models;
using ServiceLayer.DTO;
using ServiceLayer.Services;

namespace Restaurant.Controllers
{
    public class MealsController : Controller
    {
        private MealService mealService;

        public MealsController(MealService _mealService)
        {
            mealService = _mealService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<MealDTO> allMeals = await mealService.GetAllAsync();
            return View(allMeals);
        }

        public async Task<IActionResult> Add()
        {
            return View(new MealsViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddNewMeal(MealsViewModel meal)
        {
            if (ModelState.IsValid)
            {
                float weight = float.Parse(meal.Weight, CultureInfo.InvariantCulture.NumberFormat);
                float price = float.Parse(meal.Price, CultureInfo.InvariantCulture.NumberFormat);
                
                var ingredients = new List<IngredientDTO>();
                foreach (var item in meal.ingredients)
                {
                    ingredients.Add(new IngredientDTO()
                    {
                        Name = item,
                        Weight = 100
                    });
                }

                var mealDTO = new MealDTO
                {
                    Name = meal.Name,
                    Description = meal.Description,
                    Weight = weight,
                    Price = price,
                    Ingredients = ingredients
                };
                
                var createdMeal = await mealService.AddAsync(mealDTO);
                return RedirectToAction("Index", "Meals");
            }

            return RedirectToAction("Add", "Meals");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMeal(int Id)
        {
            var mealToDelete = await mealService.GetAsync(Id);
            await mealService.DeleteAsync(mealToDelete);

            return RedirectToAction("Index", "Home");
        }
    }
}
