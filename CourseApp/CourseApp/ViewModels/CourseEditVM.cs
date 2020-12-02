using CourseApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourseApp.ViewModels
{
    public class CourseEditVM
    {
        [Required]
        public Int64 Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Subject { get; set; }

        public string CourseCode { get; set; }
        public string Description { get; set; }
        public long AuthorId { get; set; }
        public Decimal Price { get; set; } 
        public long SelectedSection { get; set; }
        public List<SectionModel> Sections { get; set; } 

    }
}
