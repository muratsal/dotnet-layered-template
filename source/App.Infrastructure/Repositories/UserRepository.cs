using App.Core.Repository.Interfaces;
using App.Core.Domain;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>,IUserRepository
    {
        public UserRepository(AppDbContext context) :base(context)
        {
           
        }
        public async Task<IEnumerable<User>> GetUserWithDetailAsync(int id)
        {
           return await _context.Set<User>()
                  .Include(x => x.UserRoles)
                  .ThenInclude(x => x.Role)
                  .ThenInclude(x => x.RolePermissions)
                  .ThenInclude(x => x.Permission)
                  .Where(x => x.Id == id).ToListAsync();
        }
    }
}
