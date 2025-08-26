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
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
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
            return _dbSet.Find(id)!; 
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id) ?? throw new KeyNotFoundException();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public async Task UpdateAsync(TEntity entity)
        {
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
