using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.DTOs.User
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "IsActive is required.")]
        public bool IsActive { get; set; }
    }

}
