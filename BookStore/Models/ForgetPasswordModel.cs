using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models
{
    public class ForgetPasswordModel
    {
        [Required, EmailAddress, Display(Name = "Enter Your Register Email Address...")]
        public string Email { get; set; }
        public bool EmailSent { get; set; }
    }
}
