using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourseApp.Models
{
    [Table("Role")]
    public class RoleModel : IdentityRole<long>
    {
 
    }
}