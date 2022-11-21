using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using DomainLayer.Models;

namespace RepositoryLayer.Repository
{
    public abstract class GenericRepository<T, TKey> : IRepository<T, TKey>  where T : BaseEntity<TKey>
    {
        public ApplicationDbContext context;
        private DbSet<T> table;

        public GenericRepository(ApplicationDbContext context)
        {
            this.context = context;
            table = context.Set<T>();
        }

        public async void Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await table.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            table.Remove(entity);
            context.SaveChanges();
        }

        public T Get(TKey Id)
        {
            //await table.FindAsync(Id);
            
            return table.SingleOrDefault(x => x.Id == Id) ?? throw new InvalidOperationException("not found");
        }

        public IEnumerable<T> GetAll()
        {
            return table;
        }
        //TODO : read about specifications and IQueryable
        public IQueryable<T> GetAllQueryable()
        {
            return table.AsQueryable();
        }
        public async void SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
