using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Models;

namespace ServiceLayer.Services
{
    public interface IService<T1, T2>
        where T1 : BaseEntity
        where T2 : class
    {
        Task<T2> AddAsync(T2 entity);
        Task<T2> DeleteAsync(int id);
        Task UpdateAsync(T2 entity);
        Task<IEnumerable<T2>> GetAllAsync();
        T2 Get(int id);
        Task<T2> GetAsync(int id);

    }
}
