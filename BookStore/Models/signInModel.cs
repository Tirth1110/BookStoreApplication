using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class SignInModel
    {
        [Required, EmailAddress]
        [Display(Name = "Enter Email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Enter Password")]
        public string Password { get; set; }
        [Display(Name = "Remember Me ")]
        public bool RememberMe { get; set; }
    }
}
