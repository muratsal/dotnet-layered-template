using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Repository.Interfaces
{
    public interface IUserRepository:IRepository<User>
    {
        public  Task<User?> GetUserWithDetailAsync(int id);
        public Task<User?> GetByUsernameAsync(string userName);
        public Task<List<string>> GetUserPermissionsAsync(int id);
    }
}
