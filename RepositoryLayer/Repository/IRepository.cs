using System;
using System.Collections.Generic;
using System.Text;
using DomainLayer.Models;

namespace RepositoryLayer.Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        void Create(T entity);
        void SaveChangesAsync();
        void SaveChanges();
        void Delete(T entity);
        void Update(T entity);
        T Get(int Id);
    }
}
