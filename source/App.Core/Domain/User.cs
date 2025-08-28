using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Domain
{
    public class User : BaseEntity 
    {
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public bool IsActive { get; set; } = true;

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<UserRole> UserRoleCreatedByUsers { get; set; } = new List<UserRole>();
        public ICollection<UserRole> UserRoleUpdatedByUsers { get; set; } = new List<UserRole>();
        public ICollection<Role> RoleCreatedByUsers { get; set; } = new List<Role>();
        public ICollection<Role> RoleUpdatedByUsers { get; set; } = new List<Role>();
        public ICollection<Permission> PermissionCreatedByUsers { get; set; } = new List<Permission>();
        public ICollection<Permission> PermissionUpdatedByUsers { get; set; } = new List<Permission>();
        public ICollection<RolePermission> RolePermissionCreatedByUsers { get; set; } = new List<RolePermission>();
        public ICollection<RolePermission> RolePermissionUpdatedByUsers { get; set; } = new List<RolePermission>();
    }
}
