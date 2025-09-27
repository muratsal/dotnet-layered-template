using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.DTOs.Menu
{
  
    public class UserMenuItemDto
    {
        public string MenuName { get; set; }
        public string TranslateName { get; set; }
        public string MenuIcon { get; set; }

        public List<SubMenuItemDTO> SubMenuItems { get; set; }
    }

    public class SubMenuItemDTO
    {
        public string MenuName { get; set; }
        public string TranslateName { get; set; }
        public string State { get; set; }
        public List<SubMenuItemDTO> SubMenuItems { get; set; }
    }
}
