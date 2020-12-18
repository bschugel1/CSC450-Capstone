using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CourseApp.ViewModels
{
    public class AccountVM
    {
        public long Id { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Username")]
        public string DisplayName { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        [DisplayName("Profile Image")]
        public string ProfileImage { get; set; }
    }
}
