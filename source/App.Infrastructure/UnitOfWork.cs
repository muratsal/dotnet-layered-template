using App.Core.Interfaces.Repository;
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
        public IRoleRepository Roles { get; }
        public  IPermissionRepository Permissions { get; }

        public UnitOfWork(AppDbContext context,
                          IUserRepository users,
                          IRoleRepository roles,
                          IPermissionRepository permissions
                          )
        {
            _context = context;
            Users = users;
            Roles = roles;
            Permissions = permissions;

        }

        public int Complete() => _context.SaveChanges();
        public Task<int> CompleteAsync() => _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}
