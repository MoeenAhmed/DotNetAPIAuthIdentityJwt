﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetIdentityShared
{
    public class RegisterDTO
    {
        [Required]
        [StringLength(30, ErrorMessage = "Name should be less than 10 chars")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 5,ErrorMessage = "Password length should be between 5-15")]
        public string Password { get; set; }


        [Required]
        [StringLength(maximumLength: 30, MinimumLength = 5, ErrorMessage = "Confirm Password length should be between 5-15")]
        public string ConfirmPassword { get; set; }
    }
}
