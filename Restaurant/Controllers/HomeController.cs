using Microsoft.AspNetCore.Mvc;
using Restaurant.Models;
using System.Diagnostics;
using ServiceLayer.Services;

namespace Restaurant.Controllers
{
    public class HomeController : Controller
    {
        private MealService mealService;

        public HomeController(MealService mealService)
        {
            this.mealService = mealService;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}