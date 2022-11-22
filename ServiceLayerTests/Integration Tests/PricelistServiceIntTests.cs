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
    public class PricelistServiceIntTests
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
            pricelistService = new PricelistService(unitOfWork.Object);
        }

        private ApplicationDbContext context;
        private Mock<UnitOfWork> unitOfWork;
        private MealService mealService;
        private PricelistService pricelistService;

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
}
