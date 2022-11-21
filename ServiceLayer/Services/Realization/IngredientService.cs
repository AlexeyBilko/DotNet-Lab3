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

        public IEnumerable<IngredientDTO> GetIngredientsOfTheMeal(int mealId)
        {
            var ingredients = unitOfWork
                .IngredientRepository
                .GetIngredientInMeal(mealId)
                .Select(mapper.Map<Ingredient, IngredientDTO>);

            return ingredients;
        }

        public async Task<IngredientDTO> AddAsync(IngredientDTO entity)
        {
            var mappedEntity = mapper.Map<IngredientDTO, Ingredient>(entity);

            await unitOfWork.IngredientRepository.CreateAsync(mappedEntity);
            unitOfWork.SaveChanges();

            entity.Id = mappedEntity.Id;

            return entity;

        }

        public async Task<IngredientDTO> DeleteAsync(IngredientDTO entity)
        {
            var mappedEntity = mapper.Map<IngredientDTO, Ingredient>(entity);

            await unitOfWork.IngredientRepository.DeleteAsync(mappedEntity);
            unitOfWork.SaveChanges();

            return entity;
        }

        public async Task<IngredientDTO> UpdateAsync(IngredientDTO entity)
        {
            var mappedEntity = mapper.Map<IngredientDTO, Ingredient>(entity);

            await unitOfWork.IngredientRepository.UpdateAsync(mappedEntity);
            unitOfWork.SaveChanges();

            return entity;
        }

        public async Task<IEnumerable<IngredientDTO>> GetAllAsync()
        {
            return (await unitOfWork.IngredientRepository.GetAllAsync()).Select(mapper.Map<Ingredient, IngredientDTO>);
        }

        public async Task<IngredientDTO> GetAsync(int id)
        {
            var order = await unitOfWork.IngredientRepository.Get(id);
            if (order != null)
            {
                return mapper.Map<Ingredient, IngredientDTO>(order);
            }
            throw new ArgumentException("not found");
        }
    }
}
