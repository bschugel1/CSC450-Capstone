using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CourseApp.ViewModels
{
    public class AccountEditVM
    {
        [Required]
        public long Id { get; set; }
        [DisplayName("Profile Image")]
        public IFormFile ProfileImage { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Username")]
        public string DisplayName { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
    }
}
