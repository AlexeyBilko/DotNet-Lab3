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
	public class OrderServiceTests
	{
        [SetUp]
        public void IngredientServiceSetup()
        {
            unitOfWorkMock = new Mock<IUnitOfWork>("OrderService");
            orderService = new OrderService(unitOfWorkMock.Object);

            MapperConfiguration configuration = new MapperConfiguration(opt =>
            {
                opt.CreateMap<Order, OrderDTO>();
                opt.CreateMap<OrderDTO, Order>();
            });
            mapper = new Mapper(configuration);
        }


        protected IMapper mapper;
        protected Mock<IUnitOfWork> unitOfWorkMock;
        protected OrderService orderService;

        [Test]
        public async void GetAsync_Order_GetsCorrectItem()
        {
            var order = new Order()
            {
                Id = 1,
                TableNumber = AutoFaker.Generate<int>(),
                OrderedTime = DateTime.Now
            };

            unitOfWorkMock.Setup(esc => esc.OrderRepository.Get(order.Id).Result).Returns(order);

            OrderDTO actual = await orderService.GetAsync(order.Id);

            actual.Should().BeEquivalentTo(mapper.Map<Order, OrderDTO>(order));
        }

        [Test]
        public async void GetAllAsync_Orders_GetsCorrectItems()
        {
            var orders = new List<Order>();
            orders.Add(new Order()
            {
                Id = 1,
                TableNumber = AutoFaker.Generate<int>(),
                OrderedTime = DateTime.Now
            });
            orders.Add(new Order()
            {
                Id = 2,
                TableNumber = AutoFaker.Generate<int>(),
                OrderedTime = DateTime.Now
            });


            unitOfWorkMock.Setup(esc => esc.OrderRepository.GetAllAsync().Result.ToList()).Returns(orders);

            List<OrderDTO> actual = (await orderService.GetAllAsync()).ToList();

            actual.Should().BeEquivalentTo(orders.Select(mapper.Map<Order, OrderDTO>));
        }

        [Test]
        public async void AddAsync_Order_ItemAdded()
        {
            var order = new Order()
            {
                Id = 1,
                TableNumber = AutoFaker.Generate<int>(),
                OrderedTime = DateTime.Now
            };


            unitOfWorkMock.Setup(esc => esc.OrderRepository.CreateAsync(order).Result).Returns(order);

            OrderDTO actual = await orderService.AddAsync(mapper.Map<Order, OrderDTO>(order));

            actual.Should().BeEquivalentTo(mapper.Map<Order, OrderDTO>(order));
        }

        [Test]
        public async void UpdateAsync_Order_ItemUpdated()
        {
            var order = new Order()
            {
                Id = 1,
                TableNumber = AutoFaker.Generate<int>(),
                OrderedTime = DateTime.Now
            };


            unitOfWorkMock.Setup(esc => esc.OrderRepository.UpdateAsync(order).Result).Returns(order);

            OrderDTO actual = await orderService.UpdateAsync(mapper.Map<Order, OrderDTO>(order));

            actual.Should().BeEquivalentTo(mapper.Map<Order, OrderDTO>(order));
        }

        [Test]
        public async void DeleteAsync_Order_ItemDeleted()
        {
            var order = new Order()
            {
                Id = 1,
                TableNumber = AutoFaker.Generate<int>(),
                OrderedTime = DateTime.Now
            };


            unitOfWorkMock.Setup(esc => esc.OrderRepository.DeleteAsync(order).Result).Returns(order);

            OrderDTO actual = await orderService.DeleteAsync(mapper.Map<Order, OrderDTO>(order));

            actual.Should().BeEquivalentTo(mapper.Map<Order, OrderDTO>(order));
        }


        [Test]
        public void CreateOrder_Order_OrderCreated()
        {
            int tableNumber = AutoFaker.Generate<int>();
            DateTime createdTime = DateTime.Now;

            var order = new Order()
            {
                Id = 1,
                TableNumber = tableNumber,
                OrderedTime = createdTime
            };

            unitOfWorkMock.Setup(esc => esc.OrderRepository.CreateAsync(order).Result).Returns(order);

            OrderDTO actual = orderService.CreateOrder(tableNumber, createdTime);

            actual.Should().BeEquivalentTo(mapper.Map<Order, OrderDTO>(order));
        }

        [Test]
        public void AddMealToOrder_Meal_MealAddedToOrder()
        {
            int orderId = 1;
            int mealId = 1;
            var order = new Order()
            {
                Id = orderId,
                TableNumber = AutoFaker.Generate<int>(),
                OrderedTime = DateTime.Now
            };

            var meal = new Meal()
            {
                Id = mealId,
                Name = AutoFaker.Generate<string>(),
                Description = AutoFaker.Generate<string>(),
                Weight = AutoFaker.Generate<float>()
            };

            var mealInOrder = new MealInOrder()
            {
                Id = 1,
                MealId = mealId,
                OrderId = orderId
            };

            unitOfWorkMock.Setup(esc => esc.MealInOrderRepository.CreateAsync(mealInOrder).Result).Returns(mealInOrder);

            MealInOrder actual = orderService.AddMealToOrder(orderId, mealId);

            actual.Should().BeEquivalentTo(mapper.Map<Order, OrderDTO>(order));
        }
    }
}

