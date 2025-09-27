using App.Core.Domain;
using App.Core.Interfaces.Repository;
using App.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Infrastructure.Repositories
{
    public class PermissionRepository: Repository<Permission>, IPermissionRepository
    {
        public PermissionRepository(AppDbContext context): base(context)
        {
                
        }
    }
}
