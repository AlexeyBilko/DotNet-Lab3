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

            var context = new ApplicationDbContext(options);

            unitOfWork = new Mock<UnitOfWork>(context, new IngredientRepository(context),
                new OrderRepository(context), new MealRepository(context), new MealInOrderRepository(context),
                new PricelistRepository(context));

            mealService = new MealService(unitOfWork.Object);
            orderService = new OrderService(unitOfWork.Object);
        }

        private Mock<UnitOfWork> unitOfWork;
        private MealService mealService;
        private OrderService orderService;

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

                var actual = mealService.GetMealById(1);

                actual.Should().BeEquivalentTo(meal);
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

                var actual = orderService.GetOrderById(id);

                actual.Should().BeEquivalentTo(order);
            }
        }
    }
}
