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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}