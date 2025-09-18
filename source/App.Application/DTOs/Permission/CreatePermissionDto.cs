using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.DTOs.Permission
{
    public class CreatePermissionDto
    {
        [Required(ErrorMessage = "Permission Key is required.")]
        [StringLength(150, ErrorMessage = "Permiison Key cannot exceed 200 characters.")]
        public string Key { get; set; } = null!;

        [Required(ErrorMessage = "Description is required.")]
        [StringLength(250, ErrorMessage = "Description cannot exceed 200 characters.")]
        public string Description { get; set; } = null!;
    
    }
}
