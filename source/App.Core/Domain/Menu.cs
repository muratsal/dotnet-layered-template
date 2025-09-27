using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Domain
{
    public class Menu : BaseEntity
    {
        public string Icon { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string State { get; set; } = null!;
        public User CreatedBy { get; set; } = null!;
        public User UpdatedBy { get; set; } = null!;

    }
}
