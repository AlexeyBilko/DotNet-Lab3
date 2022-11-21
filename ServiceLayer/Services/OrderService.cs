using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DomainLayer.Models;
using RepositoryLayer.Repository;
using RepositoryLayer.UnitOfWork;
using ServiceLayer.DTO;

namespace ServiceLayer.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        protected IMapper mapper;
        public OrderService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            MapperConfiguration configuration = new MapperConfiguration(opt =>
            {
                opt.CreateMap<Order, OrderDTO>();
                opt.CreateMap<OrderDTO, Order>();
            });
            mapper = new Mapper(configuration);
        }

        public OrderDTO CreateOrder(int tableNumber, DateTime createdTime)
        {
            var order = new OrderDTO()
            {
                TableNumber = tableNumber,
                OrderedTime = createdTime
            };
            var mappedEntity = mapper.Map<OrderDTO, Order>(order);
            unitOfWork.OrderRepository.Create(mappedEntity);
            unitOfWork.SaveChanges();

            order.Id = mappedEntity.Id;
            // if some one else update record after yours creation and before line number 41 update changes will no visible 
            // for better concurtency aproach use RowVersion [optional]

            //var created = unitOfWork.OrderRepository.GetOrdersByTable(tableNumber)
            //    .FirstOrDefault(order => order.OrderedTime == createdTime);
            //if (created != null)
            //{
            //    order = mapper.Map<Order, OrderDTO>(created);
            //}

            return order;
        }

        public void AddMealToOrder(int orderId, int mealId)
        {
            if (unitOfWork.MealRepository.Get(mealId) != null||  
                unitOfWork.OrderRepository.Get(orderId) != null)
            {
                var mealToAdd = new MealInOrder()
                {
                    MealId = mealId,
                    OrderId = orderId
                };

                unitOfWork
                    .MealInOrderRepository
                    .Create(mealToAdd);
                unitOfWork.SaveChanges();
            }
        }

        public void RemoveMealFromOrder(int orderId, int mealId)
        {
            unitOfWork.MealInOrderRepository.RemoveMealFromOrder(orderId, mealId);
            unitOfWork.SaveChanges();
        }

        public IEnumerable<MealDTO> GetMealsInOder(int orderId)
        {
            var meals = unitOfWork
                .MealRepository
                .GetMealsInOrder(orderId)
                .Select(meal => mapper.Map<Meal, MealDTO>(meal));

            return meals;
        }

        public OrderDTO GetOrderById(int orderId)
        {
            var order = mapper.Map<Order, OrderDTO>(unitOfWork
                .OrderRepository
                .Get(orderId)
            );

            return order;
        }
    }
}
