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
    public class PricelistService : IPricelistService
    {
        private readonly IUnitOfWork unitOfWork;
        protected IMapper mapper;
        public PricelistService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            MapperConfiguration configuration = new MapperConfiguration(opt =>
            {
                opt.CreateMap<Pricelist, PricelistDTO>();
                opt.CreateMap<PricelistDTO, Pricelist>();
            });
            mapper = new Mapper(configuration);
        }

        public PricelistDTO GetPriceByMealId(int mealId)
        {
            var price = mapper.Map<Pricelist, PricelistDTO>(unitOfWork.PricelistRepository
                .GetPriceByMeal(mealId));

            return price;
        }

        public async Task<PricelistDTO> AddAsync(PricelistDTO entity)
        {
            var mappedEntity = mapper.Map<PricelistDTO, Pricelist>(entity);

            await unitOfWork.PricelistRepository.CreateAsync(mappedEntity);
            unitOfWork.SaveChanges();

            entity.Id = mappedEntity.Id;

            return entity;

        }

        public async Task<PricelistDTO> DeleteAsync(PricelistDTO entity)
        {
            var mappedEntity = mapper.Map<PricelistDTO, Pricelist>(entity);

            await unitOfWork.PricelistRepository.DeleteAsync(mappedEntity);
            unitOfWork.SaveChanges();

            return entity;
        }

        public async Task<PricelistDTO> UpdateAsync(PricelistDTO entity)
        {
            var mappedEntity = mapper.Map<PricelistDTO, Pricelist>(entity);

            await unitOfWork.PricelistRepository.UpdateAsync(mappedEntity);
            unitOfWork.SaveChanges();

            return entity;
        }

        public async Task<IEnumerable<PricelistDTO>> GetAllAsync()
        {
            return (await unitOfWork.PricelistRepository.GetAllAsync()).Select(mapper.Map<Pricelist, PricelistDTO>);
        }

        public async Task<PricelistDTO> GetAsync(int id)
        {
            var order = await unitOfWork.PricelistRepository.Get(id);
            if (order != null)
            {
                return mapper.Map<Pricelist, PricelistDTO>(order);
            }
            throw new ArgumentException("not found");
        }
    }
}
