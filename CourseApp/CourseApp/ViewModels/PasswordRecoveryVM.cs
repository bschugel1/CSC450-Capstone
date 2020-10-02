using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CourseApp.ViewModels
{
    public class PasswordRecoveryVM
    {
        [Required(ErrorMessage = "Email required")]
        [EmailAddress]
        public string Email { get; set; }


    }
}
