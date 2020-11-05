using System.ComponentModel.DataAnnotations.Schema;

namespace CourseApp.Models
{
    [Table("UserCourse")]
    public class UserCourseModel
    {
        public long UserId { get; set; }
        public long CourseId { get; set; }
        public UserModel User { get; set; }
        public CourseModel Course { get; set; }
    }
}