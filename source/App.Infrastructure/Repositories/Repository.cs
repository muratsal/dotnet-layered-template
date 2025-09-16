using App.Core;
using App.Core.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public TEntity Get(int id)
        {
            var entity = _dbSet.Find(id)!;
            _context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public async Task<TEntity> GetAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"{typeof(TEntity).Name} with id {id} not found.");
            _context.Entry(entity).State = EntityState.Detached;
            return entity;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllPagedAsync(
              int pageNumber,
              int pageSize,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null)
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate).ToList();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public void Add(TEntity entity, int currentUserId)
        {
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedById = currentUserId;
            entity.CreatedAt = DateTime.Now;
            entity.CreatedById = currentUserId;

            _dbSet.Add(entity);
        }

        public async Task AddAsync(TEntity entity, int currentUserId)
        {
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedById = currentUserId;
            entity.CreatedAt = DateTime.Now;
            entity.CreatedById = currentUserId;

            await _dbSet.AddAsync(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities, int currentUserId)
        {
            foreach (var item in entities)
            {
                item.UpdatedAt = DateTime.Now;
                item.UpdatedById = currentUserId;
                item.CreatedAt = DateTime.Now;
                item.CreatedById = currentUserId;
            }
            _dbSet.AddRange(entities);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, int currentUserId)
        {
            foreach (var item in entities)
            {
                item.UpdatedAt = DateTime.Now;
                item.UpdatedById = currentUserId;
                item.CreatedAt = DateTime.Now;
                item.CreatedById = currentUserId;
            }
            await _dbSet.AddRangeAsync(entities);
        }

        public void Update(TEntity entity, int currentUserId)
        {
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedById = currentUserId;

            _dbSet.Update(entity);
        }

        public async Task UpdateAsync(TEntity entity, int currentUserId)
        {
            entity.UpdatedAt = DateTime.Now;
            entity.UpdatedById = currentUserId;

            _dbSet.Update(entity);
            await Task.CompletedTask; // EF Core async update desteği yok, SaveChangesAsync sırasında commit edilir
        }

        public void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task RemoveAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await Task.CompletedTask;
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
            await Task.CompletedTask;
        }
    }
}
