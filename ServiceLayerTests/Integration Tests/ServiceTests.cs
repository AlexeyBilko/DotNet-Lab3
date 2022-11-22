using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoBogus;
using DomainLayer.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using RepositoryLayer;
using RepositoryLayer.Repository;
using RepositoryLayer.UnitOfWork;
using ServiceLayer.DTO;
using ServiceLayer.Services;

namespace ServiceLayerTests
{
    public class ServiceTests
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
            pricelistService = new PricelistService(unitOfWork.Object);
            ingredientService = new IngredientService(unitOfWork.Object);
        }

        private ApplicationDbContext context;
        private Mock<UnitOfWork> unitOfWork;
        private MealService mealService;
        private OrderService orderService;
        private PricelistService pricelistService;
        private IngredientService ingredientService;

        public class MealServiceTests : ServiceTests
        {
            [Test]
            public void GetMealById_Meal_ReturnsElement()
            {
                var meal = new MealDTO()
                {
                    Id = 1,
                    Name = AutoFaker.Generate<string>(),
                    Description = AutoFaker.Generate<string>(),
                    Weight = AutoFaker.Generate<float>()
                };

                mealService.AddAsync(meal);

                var actual = mealService.GetAsync(1);

                actual.Should().BeEquivalentTo(meal);

                context.Database.EnsureDeleted();
            }

            [Test]
            public async Task GetMealsByName_CreateTowMealsWithTheSameName_ReturnsTwoElements()
            {
                string name = AutoFaker.Generate<string>();
                var meal1 = new MealDTO()
                {
                    Id = AutoFaker.Generate<int>(),
                    Name = name,
                    Description = AutoFaker.Generate<string>(),
                    Weight = AutoFaker.Generate<float>()
                };
                var meal2 = new MealDTO()
                {
                    Id = AutoFaker.Generate<int>(),
                    Name = name,
                    Description = AutoFaker.Generate<string>(),
                    Weight = AutoFaker.Generate<float>()
                };

                await mealService.AddAsync(meal1);
                await mealService.AddAsync(meal2);

                var actual = mealService.GetMealsByName(name).ToList();

                actual.Should().ContainEquivalentOf(meal1);
                actual.Should().ContainEquivalentOf(meal2);

                await context.Database.EnsureDeletedAsync();
            }
        }

        public class OrderServiceTests : ServiceTests
        {
            [Test]
            public void CreateOrder_Order_NewElementAdded()
            {
                int id = 1;
                var order = new OrderDTO()
                {
                    Id = id,
                    TableNumber = AutoFaker.Generate<int>(),
                    OrderedTime = DateTime.Now
                };

                orderService.CreateOrder(order.TableNumber, order.OrderedTime);

                var actual = orderService.GetAsync(id);

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

        public class PricelistServiceTest : ServiceTests
        {
            [Test]
            public async Task GetPriceByMeal_Meal_GetsCorrectPrice()
            {
                int id = AutoFaker.Generate<int>();
                var meal = new MealDTO()
                {
                    Id = id,
                    Name = AutoFaker.Generate<string>(),
                    Description = AutoFaker.Generate<string>(),
                    Weight = AutoFaker.Generate<float>()
                };

                var pricelist = new PricelistDTO()
                {
                    Id = id,
                    MealId = id, 
                    Price = AutoFaker.Generate<float>()
                };

                await mealService.AddAsync(meal);
                await pricelistService.AddAsync(pricelist);
                var result = pricelistService.GetPriceByMealId(id);

                result.Should().BeEquivalentTo(pricelist);

                await context.Database.EnsureDeletedAsync();
            }
        }

        public class IngredientServiceTest : ServiceTests
        {
            [Test]
            public async Task GetIngredientById_Meal_GetsCorrectIngredient()
            {
                int id = AutoFaker.Generate<int>();
                var meal = new MealDTO()
                {
                    Id = id,
                    Name = AutoFaker.Generate<string>(),
                    Description = AutoFaker.Generate<string>(),
                    Weight = AutoFaker.Generate<float>()
                };

                var ingredient = new IngredientDTO()
                {
                    Id = id,
                    MealId = id,
                    Name = AutoFaker.Generate<string>(),
                    Weight = AutoFaker.Generate<float>()
                };

                await mealService.AddAsync(meal);
                await ingredientService.AddAsync(ingredient);
                var result = ingredientService.GetAsync(id);

                result.Should().BeEquivalentTo(ingredient);

                await context.Database.EnsureDeletedAsync();
            }

            [Test]
            public async Task GetIngredientsOfTheMeal_Meal_GetsCorrectIngredients()
            {
                int id = AutoFaker.Generate<int>();
                var meal = new MealDTO()
                {
                    Id = id,
                    Name = AutoFaker.Generate<string>(),
                    Description = AutoFaker.Generate<string>(),
                    Weight = AutoFaker.Generate<float>()
                };

                var ingredient = new IngredientDTO()
                {
                    Id = id,
                    MealId = id,
                    Name = AutoFaker.Generate<string>(),
                    Weight = AutoFaker.Generate<float>()
                };

                await mealService.AddAsync(meal);
                await ingredientService.AddAsync(ingredient);
                var result = ingredientService.GetIngredientsOfTheMeal(id).ToList();

                result[0].Should().BeEquivalentTo(ingredient);

                await context.Database.EnsureDeletedAsync();
            }
        }
    }
}
