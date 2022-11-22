using AutoBogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using RepositoryLayer;
using RepositoryLayer.Repository;
using RepositoryLayer.UnitOfWork;
using ServiceLayer.DTO;
using ServiceLayer.Services;

namespace ServiceLayerTests.Integration_Tests
{
    public class OrderServiceIntTests
    {
        [SetUp]
        public void ServiceSetup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "RestaurantTestDb")
                .Options;

            context = new ApplicationDbContext(options);

            unitOfWork = new Mock<UnitOfWork>(context, new IngredientRepository(context),
                new OrderRepository(context), new MealRepository(context), new MealInOrderRepository(context),
                new PricelistRepository(context));

            mealService = new MealService(unitOfWork.Object);
            orderService = new OrderService(unitOfWork.Object);
        }

        private ApplicationDbContext context;
        private Mock<UnitOfWork> unitOfWork;
        private MealService mealService;
        private OrderService orderService;

        [Test]
        public async Task CreateOrder_Order_NewElementAdded()
        {
            int id = 1;
            var order = new OrderDTO()
            {
                Id = id,
                TableNumber = AutoFaker.Generate<int>(),
                OrderedTime = DateTime.Now
            };

            orderService.CreateOrder(order.TableNumber, order.OrderedTime);

            var actual = await orderService.GetAsync(id);

            actual.Should().BeEquivalentTo(order);

            context.Database.EnsureDeleted();
        }

        [Test]
        public async Task AddMealToOrder_Meal_NewElementAdded()
        {
            int id = AutoFaker.Generate<int>();
            var order = new OrderDTO()
            {
                Id = id,
                TableNumber = AutoFaker.Generate<int>(),
                OrderedTime = DateTime.Now
            };
            var meal = new MealDTO()
            {
                Id = id,
                Name = AutoFaker.Generate<string>(),
                Description = AutoFaker.Generate<string>(),
                Weight = AutoFaker.Generate<float>()
            };

            orderService.CreateOrder(order.TableNumber, order.OrderedTime);
            await mealService.AddAsync(meal);

            orderService.AddMealToOrder(order.Id, meal.Id);

            var actual = context.MealInOrders.Single();

            actual.MealId.Should().Be(meal.Id);
            actual.OrderId.Should().Be(order.Id);

            await context.Database.EnsureDeletedAsync();
        }

        [Test]
        public async Task RemoveMealFromOrder_Order_ElementRemoved()
        {
            int id = AutoFaker.Generate<int>();
            var order = new OrderDTO()
            {
                Id = id,
                TableNumber = AutoFaker.Generate<int>(),
                OrderedTime = DateTime.Now
            };
            var meal = new MealDTO()
            {
                Id = id,
                Name = AutoFaker.Generate<string>(),
                Description = AutoFaker.Generate<string>(),
                Weight = AutoFaker.Generate<float>()
            };

            orderService.CreateOrder(order.TableNumber, order.OrderedTime);
            await mealService.AddAsync(meal);

            orderService.AddMealToOrder(order.Id, meal.Id);
            orderService.RemoveMealFromOrder(order.Id, meal.Id);

            context.MealInOrders.ToList().Should().BeEmpty();

            await context.Database.EnsureDeletedAsync();
        }
    }
}
