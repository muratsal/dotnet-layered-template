using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.DTOs.Role
{
    public class CreateRoleDto
    {
        [Required(ErrorMessage = "Role Name is required.")]
        [StringLength(150, ErrorMessage = "Role Name cannot exceed 150 characters.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters.")]
        public string Description { get; set; } = null!;

    }
}
