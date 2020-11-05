using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CourseApp.Helpers
{
    public static class AccessExtensions
    {
        public static bool IsCurrentAuthor(this ClaimsPrincipal user, long authorId)
        {
            return user.FindFirst(ClaimTypes.NameIdentifier).Value == authorId.ToString();            
        }
    }
}
