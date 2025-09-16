using App.Core.Repository.Interfaces;
using App.Core.Domain;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;

namespace App.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {

        }
        public async Task<User?> GetUserWithDetailAsync(int id)
        {
            return await _context.Set<User>()
                   .Include(x => x.UserRoles)
                   .ThenInclude(x => x.Role)
                   .ThenInclude(x => x.RolePermissions)
                   .ThenInclude(x => x.Permission)
                   .FirstOrDefaultAsync(x => x.Id == id);
        }


        public async Task<User?> GetByUsernameAsync(string userName)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<List<string>> GetUserPermissionsAsync(int id)
        {
            var permissions = await _context.Set<User>()
                                     .Where(u => u.Id == id)
                                     .SelectMany(u => u.UserRoles)      
                                     .SelectMany(ur => ur.Role.RolePermissions) 
                                     .Select(rp => rp.Permission.Key)  
                                     .ToListAsync();

            return permissions;
        }
    }
}
