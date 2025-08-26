using App.Core.Repository.Interfaces;
using App.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IUserRepository Users { get; }

        public UnitOfWork(AppDbContext context,
                          IUserRepository users
                          )
        {
            _context = context;
            Users = users;

        }

        public int Complete() => _context.SaveChanges();
        public Task<int> CompleteAsync() => _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}
