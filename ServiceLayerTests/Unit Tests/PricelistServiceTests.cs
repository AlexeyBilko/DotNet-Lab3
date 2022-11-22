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
	public class PricelistServiceTests
	{
        [SetUp]
        public void IngredientServiceSetup()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>("PricelistService");
            pricelistService = new PricelistService(unitOfWorkMock.Object);

            MapperConfiguration configuration = new MapperConfiguration(opt =>
            {
                opt.CreateMap<Pricelist, PricelistDTO>();
                opt.CreateMap<PricelistDTO, Pricelist>();
            });
            mapper = new Mapper(configuration);
        }


        protected IMapper mapper;
        protected Mock<IUnitOfWork> unitOfWorkMock;
        protected PricelistService pricelistService;

        [Test]
        public async void GetAsync_Order_GetsCorrectItem()
        {

            var pricelist = new Pricelist()
            {
                Id = 1,
                MealId = 1,
                Price = AutoFaker.Generate<float>()
            };

            unitOfWorkMock.Setup(esc => esc.PricelistRepository.Get(pricelist.Id).Result).Returns(pricelist);

            PricelistDTO actual = await pricelistService.GetAsync(pricelist.Id);

            actual.Should().BeEquivalentTo(mapper.Map<Pricelist, PricelistDTO>(pricelist));
        }

        [Test]
        public async void GetAllAsync_Orders_GetsCorrectItems()
        {
            var pricelists = new List<Pricelist>();
            pricelists.Add(new Pricelist()
            {
                Id = 1,
                MealId = 1,
                Price = AutoFaker.Generate<float>()
            });
            pricelists.Add(new Pricelist()
            {
                Id = 2,
                MealId = 2,
                Price = AutoFaker.Generate<float>()
            });


            unitOfWorkMock.Setup(esc => esc.PricelistRepository.GetAllAsync().Result.ToList()).Returns(pricelists);

            List<PricelistDTO> actual = (await pricelistService.GetAllAsync()).ToList();

            actual.Should().BeEquivalentTo(pricelists.Select(mapper.Map<Pricelist, PricelistDTO>));
        }

        [Test]
        public async void AddAsync_Order_ItemAdded()
        {
            var pricelist = new Pricelist()
            {
                Id = 1,
                MealId = 1,
                Price = AutoFaker.Generate<float>()
            };


            unitOfWorkMock.Setup(esc => esc.PricelistRepository.CreateAsync(pricelist).Result).Returns(pricelist);

            PricelistDTO actual = await pricelistService.AddAsync(mapper.Map<Pricelist, PricelistDTO>(pricelist));

            actual.Should().BeEquivalentTo(mapper.Map<Pricelist, PricelistDTO>(pricelist));
        }

        [Test]
        public async void UpdateAsync_Order_ItemUpdated()
        {
            var pricelist = new Pricelist()
            {
                Id = 1,
                MealId = 1,
                Price = AutoFaker.Generate<float>()
            };


            unitOfWorkMock.Setup(esc => esc.PricelistRepository.UpdateAsync(pricelist).Result).Returns(pricelist);

            PricelistDTO actual = await pricelistService.UpdateAsync(mapper.Map<Pricelist, PricelistDTO>(pricelist));

            actual.Should().BeEquivalentTo(mapper.Map<Pricelist, PricelistDTO>(pricelist));
        }

        [Test]
        public async void DeleteAsync_Order_ItemDeleted()
        {
            var pricelist = new Pricelist()
            {
                Id = 1,
                MealId = 1,
                Price = AutoFaker.Generate<float>()
            };


            unitOfWorkMock.Setup(esc => esc.PricelistRepository.DeleteAsync(pricelist).Result).Returns(pricelist);

            PricelistDTO actual = await pricelistService.DeleteAsync(mapper.Map<Pricelist, PricelistDTO>(pricelist));

            actual.Should().BeEquivalentTo(mapper.Map<Pricelist, PricelistDTO>(pricelist));
        }


        [Test]
        public void GetPriceByMealId_Price_ReturnsCorrectPricelist()
        {
            int mealId = 1;

            var pricelist = new Pricelist()
            {
                Id = 1,
                MealId = mealId,
                Price = AutoFaker.Generate<float>()
            };

            unitOfWorkMock.Setup(esc => esc.PricelistRepository.GetPriceByMeal(mealId)).Returns(pricelist);

            PricelistDTO actual = pricelistService.GetPriceByMealId(mealId);

            actual.Should().BeEquivalentTo(mapper.Map<Pricelist, PricelistDTO>(pricelist));
        }

    }
}

