using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseApp.Content
{
   public abstract class MediaItemBase
    {
        [Key]
        public long Id { get; set; }
        public string Url { get; set; }
        
    }

}