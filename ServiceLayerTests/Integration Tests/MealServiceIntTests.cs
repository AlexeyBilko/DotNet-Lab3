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
    public class MealServiceIntTests
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
        }

        private ApplicationDbContext context;
        private Mock<UnitOfWork> unitOfWork;
        private MealService mealService;

        [Test]
        public async Task GetMealById_Meal_ReturnsElement()
        {
            var meal = new MealDTO()
            {
                Id = 1,
                Name = AutoFaker.Generate<string>(),
                Description = AutoFaker.Generate<string>(),
                Weight = AutoFaker.Generate<float>()
            };

            await mealService.AddAsync(meal);

            var actual = await mealService.GetAsync(1);

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
}
