using AutoMapper;
using DomainLayer.Models;
using RepositoryLayer.Repository;

namespace ServiceLayer.Services
{
    public class GenericService<T1, T2> : IService<T1, T2>
        where T2 : class
        where T1 : BaseEntity
    {
     
        protected IRepository<T1> repository;
        protected IMapper mapper;

        public GenericService(IRepository<T1> repository)
        {
            this.repository = repository;
            MapperConfiguration configuration = new MapperConfiguration(opt =>
            {
                opt.CreateMap<T1, T2>();
                opt.CreateMap<T2, T1>();
            });
            mapper = new Mapper(configuration);
        }

        public Task<T2> AddAsync(T2 entity)
        {
            return Task.Run(() =>
            {
                T1 newEntity = mapper.Map<T2,T1>(entity);
                repository.Create(newEntity);
                repository.SaveChanges();
                return mapper.Map<T1,T2>(newEntity);
            });
        }

        public Task<T2> DeleteAsync(int id)
        {
            return Task.Run(() =>
            {
                T1 entity = repository.Get(id);
                if (entity != null)
                {
                    repository.Delete(entity);
                    repository.SaveChanges();
                }
                return mapper.Map<T1, T2>(entity);
            });
        }

        public Task<IEnumerable<T2>> GetAllAsync()
        {
            return Task.Run(() =>
            {
                IEnumerable<T2> allItems = (IEnumerable<T2>)repository
                     .GetAll()
                     .Select(x => mapper.Map<T1,T2>(x));
                return allItems;
            });
        }

        public Task UpdateAsync(T2 entity)
        {
            return Task.Run(() =>
            {
                T1 t2Entity = mapper.Map<T2, T1>(entity);
                repository.Update(t2Entity);
                repository.SaveChanges();
            });
        }

        public void Update(T2 entity)
        {
            repository.Update(mapper.Map<T2, T1>(entity));
            repository.SaveChanges();
        }

        public T2 Get(int id)
        {
            return mapper.Map<T1,T2>(repository.Get(id));
        }

        public Task<T2> GetAsync(int id)
        {
            return Task.Run(() =>
            {
                return Get(id);
            });
        }
        public void SaveChanges()
        {
            repository.SaveChanges();
        }
    }
}
