using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Repository.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        Task<TEntity> GetAsync(int id);
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity, int currentUserId);
        Task AddAsync(TEntity entity, int currentUserId);
        void AddRange(IEnumerable<TEntity> entities, int currentUserId);
        Task AddRangeAsync(IEnumerable<TEntity> entities, int currentUserId);
        void Update(TEntity entity, int currentUserId);
        Task UpdateAsync(TEntity entity, int currentUserId);
        void Remove(TEntity entity);
        Task RemoveAsync(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        Task RemoveRangeAsync(IEnumerable<TEntity> entities);
    }

}


