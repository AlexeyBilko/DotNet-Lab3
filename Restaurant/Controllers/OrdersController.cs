using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;
using Restaurant.Models;
using ServiceLayer.DTO;
using ServiceLayer.Services;

namespace Restaurant.Controllers
{
    public class OrdersController : Controller
    {
        private OrderService orderService;
        private MealService mealService;
        public OrdersController(OrderService _orderService,MealService _mealService)
        {
            orderService = _orderService;
            mealService = _mealService;
        }

        public async Task<IActionResult> Index()
        {
            List<OrdersModel> allOrders = new List<OrdersModel>();
            var orders = await orderService.GetAllAsync();
            foreach (var order in orders)
            {
                var meals = GetMealsFromOrder(order);

                allOrders.Add(new OrdersModel()
                {
                    OrderId = order.Id,
                    OrderedTime = order.OrderedTime,
                    TableNumber = order.TableNumber,
                    mealsInOrder = meals
                });
            }

            return View(allOrders);
        }

        public async Task<IActionResult> Add()
        {
            ViewBag.data = new SelectList((await mealService.GetAllAsync()).Select(m=>m.Name).ToList());
            return View(new OrdersViewModel());
        }


        [HttpPost]
        public async Task<IActionResult> AddNewOrder(OrdersViewModel order)
        {
            if (ModelState.IsValid && order.meals != null)
            {
                var orderDTO = new OrderDTO()
                {
                    TableNumber = order.TableNumber,
                    OrderedTime = order.OrderedTime
                };

                var createdOrder = await orderService.AddAsync(orderDTO);
                
                await orderService.AddMealsToOrder(createdOrder.Id, order.meals);

                return RedirectToAction("Index", "Orders");
            }

            return RedirectToAction("Add", "Orders");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int Id)
        {
            var toDelete = await orderService.GetAsync(Id);
            await orderService.DeleteAsync(toDelete);

            return RedirectToAction("Index", "Orders");
        }



        public List<MealDTO> GetMealsFromOrder(OrderDTO order)
        {
            var meals = orderService.GetMealsInOder(order.Id).ToList();
            var mealsDTO = meals.Select(meal => mealService.MealToDTO(meal)).ToList();
            return mealsDTO;
        }
    }
}
