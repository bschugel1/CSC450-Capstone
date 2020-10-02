using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseApp.Models
{
    [Table("User")]
    public class UserModel : IdentityUser<long>
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }




       // public int Status { get; set; }




        public static implicit operator IdentityUser(UserModel v)
        {
            throw new NotImplementedException();
        }
    }
}
