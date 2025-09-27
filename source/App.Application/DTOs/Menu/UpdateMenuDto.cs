using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.DTOs.Menu
{
    public class UpdateMenuDto
    {
        [Required(ErrorMessage = "Menu Id is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Icon name is required.")]
        [StringLength(150, ErrorMessage = "Icon name cannot exceed 150 characters.")]
        public string Icon { get; set; } = null!;

        [Required(ErrorMessage = "Menu name is required.")]
        [StringLength(150, ErrorMessage = "Menu name cannot exceed 250 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "State name is required.")]
        [StringLength(150, ErrorMessage = "Menu state cannot exceed 250 characters.")]
        public string State { get; set; } = null!;
    }
}
