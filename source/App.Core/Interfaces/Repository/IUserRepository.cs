using App.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Repository.Interfaces
{
    public interface IUserRepository
    {
        public  Task<IEnumerable<User>> GetUserWithDetailAsync(int id);
    }
}
