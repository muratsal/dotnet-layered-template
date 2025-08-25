using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Domain
{
    public  class Permission :BaseEntity
    {
        public string Key { get; set; } = null!;
        public string Description { get; set; } = null!;
        public User CreatedBy { get; set; } = null!;
        public User UpdatedBy { get; set; } = null!;

        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
