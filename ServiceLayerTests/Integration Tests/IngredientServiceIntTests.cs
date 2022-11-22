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
    public class IngredientServiceIntTest
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
            ingredientService = new IngredientService(unitOfWork.Object);
        }

        private ApplicationDbContext context;
        private Mock<UnitOfWork> unitOfWork;
        private MealService mealService;
        private IngredientService ingredientService;


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
            var result = await ingredientService.GetAsync(id);

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
