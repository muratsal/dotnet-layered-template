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
    public class PermisionRepository: Repository<Permission>, IPermissionRepository
    {
        public PermisionRepository(AppDbContext context): base(context)
        {
                
        }
    }
}
