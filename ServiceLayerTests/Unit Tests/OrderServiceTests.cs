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
            unitOfWorkMock = new Mock<IUnitOfWork>();
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
        public async Task GetAsync_Order_GetsCorrectItem()
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
        public async Task GetAllAsync_Orders_GetsCorrectItems()
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


            unitOfWorkMock.Setup(esc => esc.OrderRepository.GetAllAsync().Result).Returns(orders);

            List<OrderDTO> actual = (await orderService.GetAllAsync()).ToList();

            actual.Should().BeEquivalentTo(orders.Select(mapper.Map<Order, OrderDTO>));
        }

        [Test]
        public async Task AddAsync_Order_ItemAdded()
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
        public async Task UpdateAsync_Order_ItemUpdated()
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
        public async Task DeleteAsync_Order_ItemDeleted()
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
                Id = 0,
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

            var mealInOrder = new MealInOrder()
            {
                Id = 0,
                MealId = mealId,
                OrderId = orderId
            };

            unitOfWorkMock.Setup(esc => esc.MealInOrderRepository.CreateAsync(mealInOrder).Result).Returns(mealInOrder);

            MealInOrder actual = orderService.AddMealToOrder(orderId, mealId);

            MapperConfiguration configuration = new MapperConfiguration(opt =>
            {
                opt.CreateMap<MealInOrder, MealInOrderDTO>();
            });
            mapper = new Mapper(configuration);

            actual.Should().BeEquivalentTo(mapper.Map<MealInOrder, MealInOrderDTO>(mealInOrder));
        }
    }
}

