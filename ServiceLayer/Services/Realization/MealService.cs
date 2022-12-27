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
                opt.CreateMap<Ingredient, IngredientDTO>();
                opt.CreateMap<IngredientDTO, Ingredient>();
            });
            mapper = new Mapper(configuration);
        }

        public IEnumerable<MealDTO> GetMealsByName(string mealName)
        {
            var meals = unitOfWork.MealRepository
                .GetMealsByName(mealName)
                .Select(meal => MealToDTO(meal))
                .ToList();

            return meals;
        }
        
        public async Task<MealDTO> AddAsync(MealDTO entity)
        {
            //var mappedEntity = mapper.Map<MealDTO, Meal>(entity);
            var mappedEntity = MealDTOToMeal(entity);

            await unitOfWork.MealRepository.CreateAsync(mappedEntity);
            unitOfWork.SaveChanges();
            await unitOfWork.PricelistRepository.CreateAsync(new Pricelist()
                { MealId = mappedEntity.Id, Price = entity.Price });
            unitOfWork.SaveChanges();
            foreach (var ingredient in entity.Ingredients)
            {
                ingredient.MealId = mappedEntity.Id;
                await unitOfWork.IngredientRepository.CreateAsync(mapper.Map<IngredientDTO, Ingredient>(ingredient));
            }
            unitOfWork.SaveChanges();

            entity.Id = mappedEntity.Id;

            return entity;

        }

        public async Task<MealDTO> DeleteAsync(MealDTO entity)
        {
            //var mappedEntity = mapper.Map<MealDTO, Meal>(entity);
            var mappedEntity = MealDTOToMeal(entity);

            unitOfWork.MealRepository.DeleteId(mappedEntity.Id);
            //await unitOfWork.MealRepository.DeleteAsync(mappedEntity);
            unitOfWork.SaveChanges();

            return entity;
        }

        public async Task<MealDTO> UpdateAsync(MealDTO entity)
        {
            //var mappedEntity = mapper.Map<MealDTO, Meal>(entity);
            var mappedEntity = MealDTOToMeal(entity);
            await unitOfWork.MealRepository.UpdateAsync(mappedEntity);
            unitOfWork.SaveChanges();

            return entity;
        }

        public async Task<IEnumerable<MealDTO>> GetAllAsync()
        {
            return (await unitOfWork.MealRepository.GetAllAsync()).Select(MealToDTO); 
        }

        public async Task<MealDTO> GetAsync(int id)
        {
            var meal = await unitOfWork.MealRepository.Get(id);
            if (meal != null)
            {
                //return mapper.Map<Meal, MealDTO>(order);
                return MealToDTO(meal);
            }
            throw new ArgumentException("not found");
        }

        public MealDTO MealToDTO(Meal toConvert)
        {
            var price = unitOfWork.PricelistRepository.GetPriceByMeal(toConvert.Id).Price;
            var ingredients = unitOfWork.IngredientRepository
                .GetIngredientInMeal(toConvert.Id)
                .Select(mapper.Map<Ingredient, IngredientDTO>)
                .ToList();
            return new MealDTO
            {
                Id = toConvert.Id,
                Name = toConvert.Name,
                Description = toConvert.Description,
                Weight = toConvert.Weight,
                Price = price,
                Ingredients = ingredients
            };
        }

        public Meal MealDTOToMeal(MealDTO toConvert)
        {
            return new Meal
            {
                Id = toConvert.Id,
                Name = toConvert.Name,
                Description = toConvert.Description,
                Weight = toConvert.Weight
            };
        }
    }
}
