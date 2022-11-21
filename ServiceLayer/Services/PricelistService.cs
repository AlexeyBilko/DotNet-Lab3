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

        public Task<PricelistDTO> AddAsync(PricelistDTO entity)
        {
            return Task.Run(() =>
            {
                Pricelist newEntity = mapper.Map<PricelistDTO, Pricelist>(entity);
                unitOfWork.PricelistRepository.Create(newEntity);
                unitOfWork.PricelistRepository.SaveChanges();
                return mapper.Map<Pricelist, PricelistDTO>(newEntity);
            });
        }

        public Task<PricelistDTO> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(PricelistDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PricelistDTO>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public PricelistDTO Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PricelistDTO> GetAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
