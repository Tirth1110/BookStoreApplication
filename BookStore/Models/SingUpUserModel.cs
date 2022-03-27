using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class SingUpUserModel
    {
        [Required]
        [Display(Name = "Enter First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Enter Last Name")]
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Enter Date of Birth")]
        public DateTime? DateofBirth { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Enter Valid Email Address")]
        [Display(Name = "Enter Your Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Enter Your Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Enter Your Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Enter Your ConfirmPassword Same as Password")]
        public string ConfirmPassword { get; set; }
    }
}
