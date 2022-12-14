using System;
using AutoBogus;
using AutoMapper;
using DomainLayer.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using RepositoryLayer.UnitOfWork;
using ServiceLayer.DTO;
using ServiceLayer.Services;

namespace ServiceLayerTests.Unit_Tests
{
    public class IngedientServiceTests
    {
        [SetUp]
        public void IngredientServiceSetup()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            ingredientService = new IngredientService(unitOfWorkMock.Object);

            MapperConfiguration configuration = new MapperConfiguration(opt =>
            {
                opt.CreateMap<Ingredient, IngredientDTO>();
                opt.CreateMap<IngredientDTO, Ingredient>();
            });
            mapper = new Mapper(configuration);
        }


        protected IMapper mapper;
        protected Mock<IUnitOfWork> unitOfWorkMock;
        protected IngredientService ingredientService;

        [Test]
        public async Task GetAsync_Ingredient_GetsCorrectItem()
        {
            var ingredient = new Ingredient()
            {
                Id = 1,
                MealId = 1,
                Name = AutoFaker.Generate<string>(),
                Weight = AutoFaker.Generate<float>()
            };

            unitOfWorkMock.Setup(esc => esc.IngredientRepository.Get(ingredient.Id).Result).Returns(ingredient);

            IngredientDTO actual = await ingredientService.GetAsync(ingredient.Id);

            actual.Should().BeEquivalentTo(mapper.Map<Ingredient, IngredientDTO>(ingredient));
        }

        [Test]
        public async Task GetAllAsync_Ingredients_GetsCorrectItems()
        {
            var ingredients = new List<Ingredient>();
            ingredients.Add(new Ingredient()
            {
                Id = 1,
                MealId = 1,
                Name = AutoFaker.Generate<string>(),
                Weight = AutoFaker.Generate<float>()
            });
            ingredients.Add(new Ingredient()
            {
                Id = 2,
                MealId = 2,
                Name = AutoFaker.Generate<string>(),
                Weight = AutoFaker.Generate<float>()
            });


            unitOfWorkMock.Setup(esc => esc.IngredientRepository.GetAllAsync().Result).Returns(ingredients);

            List<IngredientDTO> actual = (await ingredientService.GetAllAsync()).ToList();

            actual.Should().BeEquivalentTo(ingredients.Select(mapper.Map<Ingredient, IngredientDTO>));
        }

        [Test]
        public async Task AddAsync_Ingredient_ItemAdded()
        {
            var ingredient = new Ingredient()
            {
                Id = 1,
                MealId = 1,
                Name = AutoFaker.Generate<string>(),
                Weight = AutoFaker.Generate<float>()
            };


            unitOfWorkMock.Setup(esc => esc.IngredientRepository.CreateAsync(ingredient).Result).Returns(ingredient);

            IngredientDTO actual = await ingredientService.AddAsync(mapper.Map<Ingredient, IngredientDTO>(ingredient));

            actual.Should().BeEquivalentTo(mapper.Map<Ingredient, IngredientDTO>(ingredient));
        }

        [Test]
        public async Task UpdateAsync_Ingredient_ItemUpdated()
        {
            var ingredient = new Ingredient()
            {
                Id = 1,
                MealId = 1,
                Name = AutoFaker.Generate<string>(),
                Weight = AutoFaker.Generate<float>()
            };


            unitOfWorkMock.Setup(esc => esc.IngredientRepository.UpdateAsync(ingredient).Result).Returns(ingredient);

            IngredientDTO actual = await ingredientService.UpdateAsync(mapper.Map<Ingredient, IngredientDTO>(ingredient));

            actual.Should().BeEquivalentTo(mapper.Map<Ingredient, IngredientDTO>(ingredient));
        }

        [Test]
        public async Task DeleteAsync_Ingredient_ItemDeleted()
        {
            var ingredient = new Ingredient()
            {
                Id = 1,
                MealId = 1,
                Name = AutoFaker.Generate<string>(),
                Weight = AutoFaker.Generate<float>()
            };


            unitOfWorkMock.Setup(esc => esc.IngredientRepository.DeleteAsync(ingredient).Result).Returns(ingredient);

            IngredientDTO actual = await ingredientService.DeleteAsync(mapper.Map<Ingredient, IngredientDTO>(ingredient));

            actual.Should().BeEquivalentTo(mapper.Map<Ingredient, IngredientDTO>(ingredient));
        }


        [Test]
        public void GetIngredientaOfTheMeal_Ingredients_ReturnsAllIngredients()
        {
            int mealId = 1;
            var ingredients = new List<Ingredient>();
            ingredients.Add(new Ingredient()
            {
                Id = 1,
                MealId = mealId,
                Name = AutoFaker.Generate<string>(),
                Weight = AutoFaker.Generate<float>()
            });
            ingredients.Add(new Ingredient()
            {
                Id = 2,
                MealId = mealId,
                Name = AutoFaker.Generate<string>(),
                Weight = AutoFaker.Generate<float>()
            });


            unitOfWorkMock.Setup(esc => esc.IngredientRepository.GetIngredientInMeal(mealId)).Returns(ingredients);

            List<IngredientDTO> actual = ingredientService.GetIngredientsOfTheMeal(mealId).ToList();

            actual.Should().BeEquivalentTo(ingredients.Select(mapper.Map<Ingredient, IngredientDTO>));
        }
    }
}
