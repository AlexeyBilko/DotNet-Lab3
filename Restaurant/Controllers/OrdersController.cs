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
        public OrdersController(OrderService _orderService)
        {
            orderService = _orderService;
        }
        public async Task<IActionResult> Index()
        {
            List<OrdersModel> allOrders = new List<OrdersModel>();
            var orders = await orderService.GetAllAsync();
            foreach (var order in orders)
            {
                var meals = orderService.GetMealsInOder(order.Id).ToList();
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

        public async Task<IActionResult> Add([FromServices] MealService mealService)
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

                var createdOrderl = await orderService.AddAsync(orderDTO);

                orderService.AddMealsToOrder(createdOrderl.Id, order.meals);

                return RedirectToAction("Index", "Orders");
            }

            return RedirectToAction("Add", "Orders");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(int Id)
        {
            var mealToDelete = await orderService.GetAsync(Id);
            await orderService.DeleteAsync(mealToDelete);

            return RedirectToAction("Index", "Home");
        }
    }
}
