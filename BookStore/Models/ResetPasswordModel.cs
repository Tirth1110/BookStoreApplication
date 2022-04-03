using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class ResetPasswordModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Token { get; set; }
        [Required]
        [Display(Name = "Enter New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [Display(Name = "Enter Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "Please Enter Confirm Password Same as Password...")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public bool IsSuuccess { get; set; }
    }
}
