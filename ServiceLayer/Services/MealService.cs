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

        public MealDTO GetMealById(int id)
        {
            var meal = mapper.Map<Meal, MealDTO>(
                unitOfWork.MealRepository.Get(id)
                );

            return meal;
        }

        public Task<MealDTO> AddAsync(MealDTO entity)
        {
            return Task.Run(() =>
            {
                Meal newEntity = mapper.Map<MealDTO, Meal>(entity);
                unitOfWork.MealRepository.Create(newEntity);
                unitOfWork.MealRepository.SaveChanges();
                return mapper.Map<Meal, MealDTO>(newEntity);
            });
        }

        public Task<MealDTO> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(MealDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MealDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public MealDTO Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MealDTO> GetAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
