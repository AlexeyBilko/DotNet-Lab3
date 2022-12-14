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
            unitOfWork.OrderRepository.CreateAsync(mappedEntity);
            unitOfWork.SaveChanges();

            order.Id = mappedEntity.Id;

            return order;
        }

        public MealInOrder AddMealToOrder(int orderId, int mealId)
        {
            var mealToAdd = new MealInOrder()
            {
                MealId = mealId,
                OrderId = orderId
            };

            unitOfWork
                .MealInOrderRepository
                .CreateAsync(mealToAdd);

            unitOfWork.SaveChanges();

            return mealToAdd;
        }

        public async Task<bool> AddMealsToOrder(int orderId, List<string> meals)
        {
            foreach (var meal in meals)
            {
                var _mealId = unitOfWork.MealRepository.GetMealsByName(meal).First().Id;
                var mealToAdd = new MealInOrder()
                {
                    MealId = _mealId,
                    OrderId = orderId
                };

                await unitOfWork
                    .MealInOrderRepository
                    .CreateAsync(mealToAdd);
            }

            unitOfWork.SaveChanges();

            return true;
        }

        public async void RemoveMealFromOrder(int orderId, int mealId)
        {
            Meal? mealToRemove = await unitOfWork.MealRepository.Get(mealId);
            if(mealToRemove != null)
            { 
                unitOfWork.MealInOrderRepository.FindMealAndRemoveFromOrder(mealToRemove, orderId);
                unitOfWork.SaveChanges();
            }
        }

        public IEnumerable<Meal> GetMealsInOder(int orderId)
        {
            var meals = unitOfWork
                .MealRepository
                .GetMealsInOrder(orderId);
            
            return meals;
        }

        public async Task<OrderDTO> AddAsync(OrderDTO entity)
        {
            var mappedEntity = mapper.Map<OrderDTO, Order>(entity);

            await unitOfWork.OrderRepository.CreateAsync(mappedEntity);
            unitOfWork.SaveChanges();

            entity.Id = mappedEntity.Id;

            return entity;

        }

        public async Task<OrderDTO> DeleteAsync(OrderDTO entity)
        {
            var mappedEntity = mapper.Map<OrderDTO, Order>(entity);
            var mealsInOrders = unitOfWork.MealInOrderRepository.GetAllQueryable().Where(x => x.OrderId == entity.Id).ToList();
            foreach (var item in mealsInOrders)
            {
                await unitOfWork.MealInOrderRepository.DeleteAsync(item);
            }
            //await unitOfWork.OrderRepository.DeleteAsync(mappedEntity);
            unitOfWork.OrderRepository.DeleteId(mappedEntity.Id);

            unitOfWork.SaveChanges();

            return entity;
        }

        public async Task<OrderDTO> UpdateAsync(OrderDTO entity)
        {
            var mappedEntity = mapper.Map<OrderDTO, Order>(entity);

            await unitOfWork.OrderRepository.UpdateAsync(mappedEntity);
            unitOfWork.SaveChanges();

            return entity;
        }

        public async Task<IEnumerable<OrderDTO>> GetAllAsync()
        {
            return (await unitOfWork.OrderRepository.GetAllAsync()).Select(mapper.Map<Order, OrderDTO>);
        }

        public async Task<OrderDTO> GetAsync(int id)
        {
            var order = await unitOfWork.OrderRepository.Get(id);
            if (order != null)
            {
                return mapper.Map<Order, OrderDTO>(order);
            }
            throw new ArgumentException("not found");
        }
    }
}
