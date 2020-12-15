using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseApp.Models
{
    [Table("User")]
    public class UserModel : IdentityUser<long>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public string ProfileImage { get; set; }

        public ICollection<UserCourseModel> UserCourses { get; set; }


        public static implicit operator IdentityUser(UserModel v)
        {
            throw new NotImplementedException();
        }
    }
}
