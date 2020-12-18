using Microsoft.AspNetCore.Mvc;

namespace CourseApp.Helpers
{
    public static class UrlHelperExtensions
    {
        public static string AbsoluteContent(this IUrlHelper url, string contentPath)
        {
            var request = url.ActionContext.HttpContext.Request;
            return string.Concat(request.Scheme, "://", request.Host.ToUriComponent(), request.PathBase.ToUriComponent(), url.Content(contentPath)).ToString();
        }
    }
}
