using System;
using AutoBogus;
using AutoMapper;
using DomainLayer.Models;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RepositoryLayer.UnitOfWork;
using ServiceLayer.DTO;
using ServiceLayer.Services;

namespace ServiceLayerTests.UnitTests
{
    public class IngredientServiceTests
	{
        [SetUp]
        public void IngredientServiceSetup()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>("IngredientService");
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
        public async void GetAsync_Ingredient_GetsCorrectItem()
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
    }
}

