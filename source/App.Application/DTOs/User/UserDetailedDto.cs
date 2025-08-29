using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.DTOs.User
{
    public class UserDetailedDto : UserDto
    {
        public ICollection<string> Roles { get; set; } = new List<string>();
        public ICollection<string> Permissions { get; set; } = new List<string>();
    }
}
