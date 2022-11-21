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
    public class MealService : IMealService
    {
        private readonly IUnitOfWork unitOfWork;
        protected IMapper mapper;
        public MealService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            MapperConfiguration configuration = new MapperConfiguration(opt =>
            {
                opt.CreateMap<Meal, MealDTO>();
                opt.CreateMap<MealDTO, Meal>();
            });
            mapper = new Mapper(configuration);
        }

        public IEnumerable<MealDTO> GetMealsByName(string mealName)
        {
            var meals = unitOfWork.MealRepository
                .GetMealsByName(mealName)
                .Select(meal => mapper.Map<Meal, MealDTO>(meal))
                .ToList();

            return meals;
        }
        
        public async Task<MealDTO> AddAsync(MealDTO entity)
        {
            var mappedEntity = mapper.Map<MealDTO, Meal>(entity);

            await unitOfWork.MealRepository.CreateAsync(mappedEntity);
            unitOfWork.SaveChanges();

            entity.Id = mappedEntity.Id;

            return entity;

        }

        public async Task<MealDTO> DeleteAsync(MealDTO entity)
        {
            var mappedEntity = mapper.Map<MealDTO, Meal>(entity);

            await unitOfWork.MealRepository.DeleteAsync(mappedEntity);
            unitOfWork.SaveChanges();

            return entity;
        }

        public async Task<MealDTO> UpdateAsync(MealDTO entity)
        {
            var mappedEntity = mapper.Map<MealDTO, Meal>(entity);

            await unitOfWork.MealRepository.UpdateAsync(mappedEntity);
            unitOfWork.SaveChanges();

            return entity;
        }

        public async Task<IEnumerable<MealDTO>> GetAllAsync()
        {
            return (await unitOfWork.MealRepository.GetAllAsync()).Select(mapper.Map<Meal, MealDTO>); 
        }

        public async Task<MealDTO> GetAsync(int id)
        {
            var order = await unitOfWork.MealRepository.Get(id);
            if (order != null)
            {
                return mapper.Map<Meal, MealDTO>(order);
            }
            throw new ArgumentException("not found");
        }
    }
}
