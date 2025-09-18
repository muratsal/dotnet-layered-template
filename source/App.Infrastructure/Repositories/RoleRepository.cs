using App.Core.Domain;
using App.Core.Interfaces.Repository;
using App.Core.Repository.Interfaces;
using App.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Repositories
{
    public class RoleRepository :Repository<Role>,IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {

        }
    }
}
