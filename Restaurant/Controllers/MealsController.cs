using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.UnitOfWork;
using ServiceLayer.DTO;
using ServiceLayer.Services;

namespace Restaurant.Controllers
{
    [Route("meals/")]
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

        [HttpPost]
        public async Task<IActionResult> AddNewMeal(MealDTO meal)
        {
            if (ModelState.IsValid)
            {
                var createdMeal = await mealService.AddAsync(meal);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteMeal(int id)
        {
            var mealToDelete = await mealService.GetAsync(id);
            await mealService.DeleteAsync(mealToDelete);

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> UpdateMeal(MealDTO meal)
        {
            if (ModelState.IsValid)
            {
                var updatedMeal = await mealService.UpdateAsync(meal);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}
