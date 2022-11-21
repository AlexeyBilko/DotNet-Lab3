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
    public class IngredientService : IIngredientService
    {
        private readonly IUnitOfWork unitOfWork;
        protected IMapper mapper;
        public IngredientService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            MapperConfiguration configuration = new MapperConfiguration(opt =>
            {
                opt.CreateMap<Ingredient, IngredientDTO>();
                opt.CreateMap<IngredientDTO, Ingredient>();
            });
            mapper = new Mapper(configuration);
        }

        public IngredientDTO GetIngredientById(int ingredientId)
        {
            var ingredient = mapper.Map<Ingredient, IngredientDTO>(unitOfWork
                .IngredientRepository
                .Get(ingredientId));

            return ingredient;
        }

        public IEnumerable<IngredientDTO> GetIngredientsOfTheMeal(int mealId)
        {
            var ingredients = unitOfWork
                .IngredientRepository
                .GetIngredientInMeal(mealId)
                .Select(ingredient => mapper.Map<Ingredient, IngredientDTO>(ingredient));

            return ingredients;
        }

        public Task<IngredientDTO> AddAsync(IngredientDTO entity)
        {
            return Task.Run(() =>
            {
                Ingredient newEntity = mapper.Map<IngredientDTO, Ingredient>(entity);
                unitOfWork.IngredientRepository.Create(newEntity);
                unitOfWork.IngredientRepository.SaveChanges();
                return mapper.Map<Ingredient, IngredientDTO>(newEntity);
            });
        }

        public Task<IngredientDTO> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(IngredientDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IngredientDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public IngredientDTO Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IngredientDTO> GetAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
