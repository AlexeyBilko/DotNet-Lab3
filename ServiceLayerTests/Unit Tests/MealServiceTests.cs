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

namespace ServiceLayerTests.UnitTests
{
	public class MealServiceTests
	{
        [SetUp]
        public void IngredientServiceSetup()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>();
            mealService = new MealService(unitOfWorkMock.Object);

            MapperConfiguration configuration = new MapperConfiguration(opt =>
            {
                opt.CreateMap<Meal, MealDTO>();
                opt.CreateMap<MealDTO, Meal>();
            });
            mapper = new Mapper(configuration);
        }


        protected IMapper mapper;
        protected Mock<IUnitOfWork> unitOfWorkMock;
        protected MealService mealService;

        [Test]
        public async Task GetAsync_Meal_GetsCorrectItem()
        {
            var meal = new Meal()
            {
                Id = 1,
                Name = AutoFaker.Generate<string>(),
                Description = AutoFaker.Generate<string>(),
                Weight = AutoFaker.Generate<float>()
            };

            unitOfWorkMock.Setup(esc => esc.MealRepository.Get(meal.Id).Result).Returns(meal);

            MealDTO actual = await mealService.GetAsync(meal.Id);

            actual.Should().BeEquivalentTo(mapper.Map<Meal, MealDTO>(meal));
        }

        [Test]
        public async Task GetAllAsync_Meals_GetsCorrectItems()
        {
            var meals = new List<Meal>();
            meals.Add(new Meal()
            {
                Id = 1,
                Name = AutoFaker.Generate<string>(),
                Description = AutoFaker.Generate<string>(),
                Weight = AutoFaker.Generate<float>()
            });
            meals.Add(new Meal()
            {
                Id = 2,
                Name = AutoFaker.Generate<string>(),
                Description = AutoFaker.Generate<string>(),
                Weight = AutoFaker.Generate<float>()
            });


            unitOfWorkMock.Setup(esc => esc.MealRepository.GetAllAsync().Result).Returns(meals);

            List<MealDTO> actual = (await mealService.GetAllAsync()).ToList();

            actual.Should().BeEquivalentTo(meals.Select(mapper.Map<Meal, MealDTO>));
        }

        [Test]
        public async Task AddAsync_Meal_ItemAdded()
        {
            var meal = new Meal()
            {
                Id = 1,
                Name = AutoFaker.Generate<string>(),
                Description = AutoFaker.Generate<string>(),
                Weight = AutoFaker.Generate<float>()
            };


            unitOfWorkMock.Setup(esc => esc.MealRepository.CreateAsync(meal).Result).Returns(meal);

            MealDTO actual = await mealService.AddAsync(mapper.Map<Meal, MealDTO>(meal));

            actual.Should().BeEquivalentTo(mapper.Map<Meal, MealDTO>(meal));
        }

        [Test]
        public async Task UpdateAsync_Meal_ItemUpdated()
        {
            var meal = new Meal()
            {
                Id = 1,
                Name = AutoFaker.Generate<string>(),
                Description = AutoFaker.Generate<string>(),
                Weight = AutoFaker.Generate<float>()
            };


            unitOfWorkMock.Setup(esc => esc.MealRepository.UpdateAsync(meal).Result).Returns(meal);

            MealDTO actual = await mealService.UpdateAsync(mapper.Map<Meal, MealDTO>(meal));

            actual.Should().BeEquivalentTo(mapper.Map<Meal, MealDTO>(meal));
        }

        [Test]
        public async Task DeleteAsync_Meal_ItemDeleted()
        {
            var meal = new Meal()
            {
                Id = 1,
                Name = AutoFaker.Generate<string>(),
                Description = AutoFaker.Generate<string>(),
                Weight = AutoFaker.Generate<float>()
            };


            unitOfWorkMock.Setup(esc => esc.MealRepository.DeleteAsync(meal).Result).Returns(meal);

            MealDTO actual = await mealService.DeleteAsync(mapper.Map<Meal, MealDTO>(meal));

            actual.Should().BeEquivalentTo(mapper.Map<Meal, MealDTO>(meal));
        }


        [Test]
        public void GetMealsByName_Meals_ReturnsAllMeals()
        {
            string mealName = AutoFaker.Generate<string>();
            var meals = new List<Meal>();
            meals.Add(new Meal()
            {
                Id = 1,
                Name = mealName,
                Description = AutoFaker.Generate<string>(),
                Weight = AutoFaker.Generate<float>()
            });
            meals.Add(new Meal()
            {
                Id = 2,
                Name = mealName,
                Description = AutoFaker.Generate<string>(),
                Weight = AutoFaker.Generate<float>()
            });


            unitOfWorkMock.Setup(esc => esc.MealRepository.GetMealsByName(mealName)).Returns(meals.AsQueryable());

            List<MealDTO> actual = mealService.GetMealsByName(mealName).ToList();

            actual.Should().BeEquivalentTo(meals.Select(mapper.Map<Meal, MealDTO>));
        }
    }
}

