﻿
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CourseApp.ViewModels
{
    public class RegistrationVM
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email required")]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [DisplayName("Username")]
        public string PersonUsername { get; set; }
        [Required(ErrorMessage = "Password required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The two passwords must match!")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
    }
}
