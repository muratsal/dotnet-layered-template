using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Domain
{
    public class RolePermission:BaseEntity
    {
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public int PermissionId { get; set; }
        public Permission Permission { get; set; } = null!;

        public User CreatedBy { get; set; } = null!;
        public User UpdatedBy { get; set; } = null!;
    }
}
