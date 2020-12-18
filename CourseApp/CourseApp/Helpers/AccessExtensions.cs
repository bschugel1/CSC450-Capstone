using System.Security.Claims;

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
