using CourseApp.Models;
using Microsoft.AspNetCore.Http;

namespace CourseApp.ViewModels
{
    public class FileUploadVM
    {
        public long Id { get; set; }
        public long CourseId { get; set; }
        public long? ParentSectionId { get; set; }

        public string Title { get; set; }
        public string Type { get; set; }
        public MediaItemModel Items { get; set; }
        public IFormFile FormFile { get; set; }
    }
}
